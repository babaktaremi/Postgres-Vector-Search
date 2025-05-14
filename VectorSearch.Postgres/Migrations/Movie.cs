using System;
using System.Collections.Generic;

namespace VectorSearch.Postgres.Migrations;

public partial class Movie
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public long? ParentId { get; set; }

    public DateOnly? Date { get; set; }

    public long? SeriesId { get; set; }

    public int? Runtime { get; set; }

    public decimal? Budget { get; set; }

    public decimal? Revenue { get; set; }

    public string? Homepage { get; set; }

    public decimal? VoteAverage { get; set; }

    public long? VotesCount { get; set; }

    public virtual ICollection<Movie> InverseParent { get; set; } = new List<Movie>();

    public virtual ICollection<Movie> InverseSeries { get; set; } = new List<Movie>();

    public virtual MovieAbstractsEn? MovieAbstractsEn { get; set; }

    public virtual Movie? Parent { get; set; }

    public virtual Movie? Series { get; set; }
}
