using System;
using System.ComponentModel.DataAnnotations;  //Entity Framework Namespace - DbContext | DbSet
using System.Data.Entity;

namespace MVC5_Sample.Models
{
    public class Movie
    {
        public int ID { get; set; }

        [StringLength(60, MinimumLength=3)]
        public string Title { get; set; }
        
        [Display(Name="Release Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy HH:mm:ss}", ApplyFormatInEditMode=true)]
        public DateTime ReleaseDate { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
        [Required]
        [StringLength(30)]
        public string Genre { get; set; }

        [Range(1,100)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
        [StringLength(5)]
        public string Rating { get; set; }
    }

    // Entity Framework - Database Context
    // FETCH, INSERT and UPDATE
    public class MovieDBContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
    }
}