using MangaVault.Models;
using MangaVault.Services;
using Microsoft.AspNetCore.Mvc;

namespace MangaVault.Controllers
{
    public class GenresController : Controller
    {
        private readonly IGenreService _genreService;

        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        public async Task<IActionResult> Index()
        {
            var genres = await _genreService.GetAllAsync();
            return View(genres);
        }

        [HttpGet]
        public IActionResult Create() => View(new Genre());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Genre genre)
        {
            if (await _genreService.NameExistsAsync(genre.Name))
            {
                ModelState.AddModelError(nameof(Genre.Name), "This genre already exists.");
            }

            if (!ModelState.IsValid)
            {
                return View(genre);
            }

            await _genreService.CreateAsync(genre);
            return RedirectToAction(nameof(Index));
        }
    }
}
