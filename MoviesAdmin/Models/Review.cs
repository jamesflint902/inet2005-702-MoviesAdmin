using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MoviesAdmin.Models
{
    
    public class Review

    {
        // primary
        public int Id { get; set; }

        //foreign key to movies
        [Display(Name = "Movie")]
        public int MovieId { get; set; }


        //  nav prop
        [ValidateNever]
        public Movie Movie { get; set; } = null!;


        [Required]
        [StringLength(1000, ErrorMessage = "Review cannot exceed 1000 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(1,5)]
        [Display(Name = "Rating (1-5)")]
        public int  Rating { get; set; }

        [Required]
        [Display(Name = "Published")]
        public bool IsPublished { get; set; }


        [Display(Name = "Created By")]
        public string CreatedBy { get; set; } = string.Empty;

        [Display(Name = "Created Date")]
        public DateOnly CreatedDate { get; set; }


    }
}
