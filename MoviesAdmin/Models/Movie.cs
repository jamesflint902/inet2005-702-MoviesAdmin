using System;
using System.ComponentModel.DataAnnotations;

namespace MoviesAdmin.Models
{
    public class Movie
    {
        public int Id { get; set; }


     
        [Required]
        [StringLength(200, MinimumLength = 1)]   // accept 1 - 200 chars
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 1)]   // accept 1 - 50 chars
        public string Genre { get; set; } = string.Empty;

        [Required]
        [RegularExpression("G|PG|PG-13|R")]  // only received accepted ffromating for rting
        public string Rating {  get; set; } = string.Empty;

        [Required]
        [StringLength(2000, MinimumLength = 10)]  // accept from 10 - 2000 chars
        public string Description {  get; set; } = string.Empty;

        [Required]
        [Range(1, 51500)]  // runtime up to logest movie ever made 
        public int Runtime { get; set; }

        [Required]
        [Range(typeof(DateTime), "1/1/1888", "12/31/2100")] // range from "first" movie made to somewhere in the future 
        public DateTime ReleaseDate { get; set; }

   
        //required property or set to null

    }
}
