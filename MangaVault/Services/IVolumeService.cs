using MangaVault.ViewModels;

namespace MangaVault.Services
{
    public interface IVolumeService
    {
        Task<VolumeFormViewModel?> GetForCreateAsync(int seriesId);
        Task<VolumeFormViewModel?> GetForEditAsync(int id);

        Task<IReadOnlyDictionary<string, string>> CheckRulesAsync(VolumeFormViewModel model);

        Task CreateAsync(VolumeFormViewModel model);
        Task<bool> UpdateAsync(VolumeFormViewModel model);
        Task<int?> DeleteAsync(int id);
    }
}
