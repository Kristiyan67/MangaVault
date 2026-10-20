using MangaVault.Data;
using MangaVault.Models;
using Microsoft.EntityFrameworkCore;

namespace MangaVault.Services
{
    public class GenreService : IGenreService
    {
        private readonly ApplicationDbContext _context;

        public GenreService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Genre>> GetAllAsync() =>
            await _context.Genres
            .AsNoTracking()
            .OrderBy(g => g.Name)
            .ToListAsync();

        public async Task<bool> NameExistsAsync(string name) =>
            await _context.Genres.AnyAsync(g => g.Name == name.Trim());

        public async Task CreateAsync(Genre genre)
        {
            genre.Name = genre.Name.Trim();
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();
        }
    }
}
