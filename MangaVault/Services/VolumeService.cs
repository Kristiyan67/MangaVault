using MangaVault.Data;
using MangaVault.Models;
using MangaVault.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MangaVault.Services
{
    public class VolumeService : IVolumeService
    {
        private readonly ApplicationDbContext _context;

        public VolumeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<VolumeFormViewModel?> GetForCreateAsync(int seriesId)
        {
            var title = await _context.Series
                .AsNoTracking()
                .Where(s => s.Id == seriesId)
                .Select(s => s.Title)
                .FirstOrDefaultAsync();

            if (title is null)
            {
                return null;
            }

            return new VolumeFormViewModel { SeriesId = seriesId, SeriesTitle = title };
        }

        public async Task<VolumeFormViewModel?> GetForEditAsync(int id)
        {
            return await _context.Volumes
                .AsNoTracking()
                .Where(v => v.Id == id)
                .Select(v => new VolumeFormViewModel
                {
                    Id = v.Id,
                    SeriesId = v.SeriesId,
                    SeriesTitle = v.Series.Title,
                    Number = v.Number,
                    ReleaseDate = v.ReleaseDate,
                    IsOwned = v.IsOwned,
                    PurchasePrice = v.PurchasePrice
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyDictionary<string, string>> CheckRulesAsync(VolumeFormViewModel model)
        {
            var errors = new Dictionary<string, string>();

            var series = await _context.Series
                .AsNoTracking()
                .Where(s => s.Id == model.SeriesId)
                .Select(s => new { s.TotalVolumes })
                .FirstOrDefaultAsync();

            if (series is null)
            {
                errors[string.Empty] = "The series for this volume does not exist.";
                return errors;
            }

            // rule 1; num cant exceed the series total
            if (series.TotalVolumes.HasValue && model.Number > series.TotalVolumes.Value)
            {
                errors[nameof(model.Number)] =
                    $"This series has only {series.TotalVolumes} volumes.";
                return errors;
            }

            // rule 2; bez povtorni nomera v sushtite series
            var duplicate = await _context.Volumes.AnyAsync(v =>
               v.SeriesId == model.SeriesId &&
                v.Number == model.Number &&
                v.Id != model.Id);

            if (duplicate)
            {
                errors[nameof(model.Number)] =
                    $"Volume {model.Number} already exists in this series.";
            }

            return errors;
        }

        public async Task CreateAsync(VolumeFormViewModel model)
        {
            var volume = new Volume
            {
                SeriesId = model.SeriesId,
                Number = model.Number,
                ReleaseDate = model.ReleaseDate,
                IsOwned = model.IsOwned,
                PurchasePrice = model.IsOwned ? model.PurchasePrice : null
            };

            _context.Volumes.Add(volume);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(VolumeFormViewModel model)
        {
            var volume = await _context.Volumes.FirstOrDefaultAsync(v => v.Id == model.Id);
            if (volume is null)
            {
                return false;
            }

            volume.Number = model.Number;
            volume.ReleaseDate = model.ReleaseDate;
            volume.IsOwned = model.IsOwned;
            volume.PurchasePrice = model.IsOwned ? model.PurchasePrice : null;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int?> DeleteAsync(int id)
        {
            var volume = await _context.Volumes.FirstOrDefaultAsync(v => v.Id == id);
            if (volume is null)
            {
                return null;
            }

            var seriesId = volume.SeriesId;
            _context.Volumes.Remove(volume);
            await _context.SaveChangesAsync();
            return seriesId;
        }
    }
}
