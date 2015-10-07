using System;
using System.Data.Entity;  //Entity Framework Namespace - DbContext | DbSet

namespace MVC5_Sample.Models
{
    public class Movie
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }
    }

    // Entity Framework - Database Context
    // FETCH, INSERT and UPDATE
    public class MovieDBContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
    }
}