using System;
using System.ComponentModel.DataAnnotations;

namespace MoviesAdmin.Models
{
    public class Movie
    {
        public int Id { get; set; }


        [Length()]
        [Required]
        public string Title { get; set; } = string.Empty;

        public string Genre { get; set; } = string.Empty;

        public string Rating {  get; set; } = string.Empty;

        public string Description {  get; set; } = string.Empty;

        public int Runtime { get; set; }

        public DateTime ReleaseDate { get; set; }

        //public required string ImageURL { get; set; }
        //required property or set to null

    }
}
