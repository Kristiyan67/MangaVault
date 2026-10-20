using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MangaVault.Models
{
    public class Volume
    {
        public const int MinNumber = 1;
        public const int MaxNumber = 500;

        public int Id { get; set; }

        [Range(MinNumber, MaxNumber, ErrorMessage = "Volume number must be between {1} and {2}.")]
        public int Number { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ReleaseDate { get; set; }

        public bool IsOwned { get; set; }

        [Range(0, 999.99, ErrorMessage = "Price must be between {1} and {2}.")]
        [Column(TypeName = "decimal(8,2")]
        public decimal? PurchasePrice { get; set; }

        public int SeriesId { get; set; }
        public Series Series { get; set; } = null;
    }
}
