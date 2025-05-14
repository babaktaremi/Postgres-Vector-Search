using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using VectorSearch.Postgres.Migrations;

namespace VectorSearch.Postgres.Context;

public partial class MoviesDbContext : DbContext
{
    public MoviesDbContext()
    {
    }

    public MoviesDbContext(DbContextOptions<MoviesDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<MovieAbstractsEn> MovieAbstractsEns { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("kind", new[] { "movie", "series", "season", "episode", "movieseries" })
            .HasPostgresExtension("tsm_system_rows");

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("movies_pkey");

            entity.ToTable("movies");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Budget).HasColumnName("budget");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Homepage).HasColumnName("homepage");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.ParentId).HasColumnName("parent_id");
            entity.Property(e => e.Revenue).HasColumnName("revenue");
            entity.Property(e => e.Runtime).HasColumnName("runtime");
            entity.Property(e => e.SeriesId).HasColumnName("series_id");
            entity.Property(e => e.VoteAverage).HasColumnName("vote_average");
            entity.Property(e => e.VotesCount).HasColumnName("votes_count");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("movies_parent_id_fkey");

            entity.HasOne(d => d.Series).WithMany(p => p.InverseSeries)
                .HasForeignKey(d => d.SeriesId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("movies_series_id_fkey");
        });

        modelBuilder.Entity<MovieAbstractsEn>(entity =>
        {
            entity.HasKey(e => e.MovieId).HasName("movie_abstracts_en_pkey");

            entity.ToTable("movie_abstracts_en");

            entity.Property(e => e.MovieId)
                .ValueGeneratedNever()
                .HasColumnName("movie_id");
            entity.Property(e => e.Abstract).HasColumnName("abstract");

            entity.HasOne(d => d.Movie).WithOne(p => p.MovieAbstractsEn)
                .HasForeignKey<MovieAbstractsEn>(d => d.MovieId)
                .HasConstraintName("movie_abstracts_en_movie_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
