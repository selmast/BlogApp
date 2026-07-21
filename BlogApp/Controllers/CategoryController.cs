using BlogApp.Models;
using BlogApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly BlogDbContext _db;
        private readonly ITranslationService _translationService;

        public CategoryController(BlogDbContext db, ITranslationService translationService)
        {
            _db = db;
            _translationService = translationService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _db.Categories.ToListAsync();
            return View(categories);
        }

        // POST: /Category/TranslateFields  -- called via AJAX from Create/Edit forms
        [HttpPost]
        public async Task<IActionResult> TranslateFields([FromBody] TranslateCategoryRequest request)
        {
            var nameEn = await _translationService.TranslateAsync(request.Name, "EN");
            var descriptionEn = string.IsNullOrWhiteSpace(request.Description)
                ? ""
                : await _translationService.TranslateAsync(request.Description, "EN");

            return Json(new { nameEn, descriptionEn });
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Add(category);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _db.Categories.FindAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Update(category);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var category = await _db.Categories.FindAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _db.Categories.FindAsync(id);
            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }

    public class TranslateCategoryRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}