using System.ComponentModel.DataAnnotations;
using MangaVault.Models.Enums;

namespace MangaVault.Models
{
    public class Series
    {
        public const int TitleMinLength = 1;
        public const int TitleMaxLength = 150;
        public const int DescriptionMaxLength = 2000;
        public const int CoverUrlMaxLength = 500;
        public const int NotesMaxLength = 1000;
        public const int MinRating = 1;
        public const int MaxRating = 10;
        public const int MaxTotalVolumes = 100;

        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength,
            ErrorMessage = "Title must be between {2} and {1} characters.")]
        public string Title { get; set; } = string.Empty;

        [StringLength(DescriptionMaxLength)]
        public string? Description { get; set; }

        [Url(ErrorMessage = "Please enter a valid URL.")]
        [StringLength(CoverUrlMaxLength)]
        public string? CoverUrl { get; set; }

        public PublicationStatus PublicationStatus { get; set; }

        [Range(1, MaxTotalVolumes, ErrorMessage = "Total volumes must be between {1} and {2}.")]
        public int? TotalVolumes { get; set; }

        public ReadingStatus ReadingStatus { get; set; }

        [Range(MinRating, MaxRating, ErrorMessage = "Rating must be between {1} and {2}.")]
        public int? Rating { get; set; }

        [StringLength(NotesMaxLength)]
        public string? Notes { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; } = null;

        public ICollection<Volume> Volumes { get; set; } = new List<Volume>();
        public ICollection<SeriesGenre> SeriesGenre { get; set; } = new List<SeriesGenre>();
    }
}
