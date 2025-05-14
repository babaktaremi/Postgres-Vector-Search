using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Pgvector;
using Pgvector.EntityFrameworkCore;
using VectorSearch.Postgres.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<MoviesDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("MoviesDb"),o => o.UseVector());
});

builder.Services.AddEmbeddingGenerator(new OllamaEmbeddingGenerator(builder.Configuration["Ollama:Url"],"mxbai-embed-large"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/CalculateVectors",
    async (MoviesDbContext db, IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator) =>
    {
        var moviesDescriptions = await db.MovieAbstractsEns.ToArrayAsync();

        await Parallel.ForEachAsync(moviesDescriptions, async (moviesDescription, _) =>
        {
            if (string.IsNullOrEmpty(moviesDescription.Abstract))
                return;

            var generatedEmbeddings = await embeddingGenerator.GenerateAsync([moviesDescription.Abstract]);

            moviesDescription.Embedding = new Vector(generatedEmbeddings.Single().Vector);
        });
      
        await db.SaveChangesAsync();
    });

app.MapGet("/GetByDescription",
    async (string description, MoviesDbContext db, IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator) =>
    {
        var embeddedDescription = await embeddingGenerator.GenerateAsync([description]);
        var embedding = new Vector(embeddedDescription.Single().Vector);
        var movies = await db.MovieAbstractsEns.Include(c => c.Movie)
            .Where(c => c.Embedding != null && c.Embedding.CosineDistance(embedding) < 0.4f)
            .Select(c => c.Movie)
            .ToArrayAsync();

        return movies.Select(c => new
        {
            c.Id,
            c.Name,
            c.Budget
        });
    });


app.Run();

