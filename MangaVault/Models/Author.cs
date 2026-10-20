using System.ComponentModel.DataAnnotations;

namespace MangaVault.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public ICollection<Series> Series { get; set; } = new List<Series>();
    }
}
