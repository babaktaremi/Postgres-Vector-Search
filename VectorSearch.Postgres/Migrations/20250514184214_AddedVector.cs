using Microsoft.EntityFrameworkCore.Migrations;
using Pgvector;

#nullable disable

namespace VectorSearch.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AddedVector : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Vector>(
                name: "Embedding",
                table: "movie_abstracts_en",
                type: "vector",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Embedding",
                table: "movie_abstracts_en");
        }
    }
}
