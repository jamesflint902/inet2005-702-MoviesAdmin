using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MoviesAdmin.Models
{
    public class Review

    {
        // primary
        public int Id { get; set; }

        //foreign key to movies
        public int MovieId { get; set; }


        //  nav prop
        [ValidateNever]
        public Movie Movie { get; set; } = null!;


        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(1,5)]
        public int  Rating { get; set; }

        [Required]
        public bool IsPublished { get; set; }
        


        public string CreatedBy { get; set; } = string.Empty;

        [ScaffoldColumn(false)]
        public DateOnly CreatedDate { get; set; }


    }
}
