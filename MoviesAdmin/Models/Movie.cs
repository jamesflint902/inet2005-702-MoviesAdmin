using System;
using System.ComponentModel.DataAnnotations;

namespace MoviesAdmin.Models
{
    public class Movie
    {
        public int Id { get; set; }


     
        [Required]
        [Display(Prompt = "Enter the movie title")]
        [StringLength(200, MinimumLength = 1)]   // accept 1 - 200 chars
        public string Title { get; set; } = string.Empty;

        [Required]
        [Display(Prompt = "e.g., Action, Drama, Sci‑Fi")]
        [StringLength(50, MinimumLength = 1)]   // accept 1 - 50 chars
        public string Genre { get; set; } = string.Empty;

        [Required]
        [Display(Prompt = "Rating: G, PG, PG‑13, or R")]
        [RegularExpression("G|PG|PG-13|R")]  // only received accepted ffromating for rting
        public string Rating {  get; set; } = string.Empty;

        [Required]
        [Display(Prompt = "Write a short summary of the movie")]
        [StringLength(2000, MinimumLength = 10)]  // accept from 10 - 2000 chars
        public string Description {  get; set; } = string.Empty;

        [Required]
        [Display(Prompt = "Total runtime in minutes")]
        [Range(1, 51500)]  // runtime up to logest movie ever made 
        public int Runtime { get; set; }

        [Required]
        [Display(Prompt = "YYYY MM DD")]
        /* Changed DateTime To DateOnly and re-migrated */
        public DateOnly ReleaseDate { get; set; }

   
        //required property or set to null

    }
}
