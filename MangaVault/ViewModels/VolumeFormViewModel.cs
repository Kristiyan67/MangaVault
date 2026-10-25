using System.ComponentModel.DataAnnotations;
using MangaVault.Models;
using MangaVault.Validation;

namespace MangaVault.ViewModels
{
    public class VolumeFormViewModel : IValidatableObject
    {
        public int Id { get; set; }
        public int SeriesId { get; set; }
        public string? SeriesTitle { get; set; }

        [Range(Volume.MinNumber, Volume.MaxNumber, ErrorMessage = "Volume number must be between {1} and {2}.")]
        public int Number { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [NotInFutureIfOwned(nameof(IsOwned))]
        public DateTime? ReleaseDate { get; set; }

        public bool IsOwned { get; set; }

        [Range(0, 999.99, ErrorMessage ="Price must be between {1} and {2}.")]
        public decimal? PurchasePrice { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!IsOwned && PurchasePrice.HasValue)
            {
                yield return new ValidationResult(
                    "Only owned volumes can have a purchase price.",
                    new[] { nameof(PurchasePrice) });
            }
        }
    }
}
