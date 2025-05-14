using System;
using System.Collections.Generic;
using Pgvector;


namespace VectorSearch.Postgres.Migrations;

public partial class MovieAbstractsEn
{
    public long MovieId { get; set; }

    public string? Abstract { get; set; }

    public virtual Movie Movie { get; set; } = null!;
    
    public Vector? Embedding { get; set; }
    
}
