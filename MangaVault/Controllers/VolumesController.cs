using MangaVault.Services;
using MangaVault.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MangaVault.Controllers
{
    public class VolumesController :Controller
    {
        private readonly IVolumeService _volumeService;

        public VolumesController(IVolumeService volumeService)
        {
            _volumeService = volumeService;
        }

        [HttpGet]
        public async Task<IActionResult> Create(int seriesId)
        {
            var model = await _volumeService.GetForCreateAsync(seriesId);
            if (model is null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VolumeFormViewModel model)
        {
            foreach (var (field, message) in await _volumeService.CheckRulesAsync(model))
            {
                ModelState.AddModelError(field, message);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _volumeService.CreateAsync(model);
            }
            catch (DbUpdateException ex) when (IsDuplicateKey(ex))
            {
                ModelState.AddModelError(nameof(model.Number),
                    $"Volume {model.Number} already exists in this series.");
                return View(model);
            }

            return RedirectToAction("Details", "Series", new { id = model.SeriesId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _volumeService.GetForEditAsync(id);
            if (model is null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VolumeFormViewModel model)
        {
            foreach (var (field, message) in await _volumeService.CheckRulesAsync(model))
            {
                ModelState.AddModelError(field, message);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var updated = await _volumeService.UpdateAsync(model);
                if (!updated)
                {
                    return NotFound();
                }
            }
            catch (DbUpdateException ex) when (IsDuplicateKey(ex))
            {
                ModelState.AddModelError(nameof(model.Number),
                    $"Volume {model.Number} already exists in this series.");
                return View(model);
            }

            return RedirectToAction("Details", "Series", new { id = model.SeriesId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var seriesId = await _volumeService.DeleteAsync(id);
            if (seriesId is null)
            {
                return NotFound();
            }

            return RedirectToAction("Details", "Series", new { id = seriesId });
        }

        private static bool IsDuplicateKey(DbUpdateException ex) =>
            ex.InnerException is SqlException { Number: 2601 or 2627 };
    }
}
