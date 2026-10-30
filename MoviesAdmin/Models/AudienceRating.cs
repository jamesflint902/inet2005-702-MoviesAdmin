using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MoviesAdmin.Models
{
    public class AudienceRating
    {
        public int Id { get; set; }

        [Required]
        [RegularExpression("G|PG|PG-13|R")]
        public string Rating { get; set; } = string.Empty;

        [ValidateNever]
        public List<Movie>? Movies { get; set; }
    }
}