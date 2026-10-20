using MangaVault.Models;

namespace MangaVault.Services
{
    public interface IGenreService
    {
        Task<IReadOnlyList<Genre>> GetAllAsync();
        Task<bool> NameExistsAsync(string name);
        Task CreateAsync(Genre genre);
    }
}
