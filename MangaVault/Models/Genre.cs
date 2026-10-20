using System.ComponentModel.DataAnnotations;

namespace MangaVault.Models
{
    public class Genre
    {
        public const int NameMinLength = 2;
        public const int NameMaxLength = 50;

        public int Id { get; set; }

        [Required(ErrorMessage = "Genre name is required.")]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, 
           ErrorMessage = "Name must be between {2} and {1} characters.")]
        public string Name { get; set; } = string.Empty;

        public ICollection<SeriesGenre> SeriesGenres { get; set; } = new List<SeriesGenre>();
    }
}
