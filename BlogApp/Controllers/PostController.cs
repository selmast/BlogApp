using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly BlogDbContext _db;

        public PostController(BlogDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _db.Posts.Include(p => p.Category).ToListAsync();
            return View(posts);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var post = await _db.Posts
                .Include(p => p.Category)
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null) return NotFound();
            return View(post);
        }

        public IActionResult Create()
        {
            ViewBag.CategoryList = new SelectList(_db.Categories.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Post post)
        {
            if (ModelState.IsValid)
            {
                _db.Posts.Add(post);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CategoryList = new SelectList(_db.Categories.ToList(), "Id", "Name");
            return View(post);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var post = await _db.Posts.FindAsync(id);
            if (post == null) return NotFound();
            ViewBag.CategoryList = new SelectList(_db.Categories.ToList(), "Id", "Name");
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Post post)
        {
            if (ModelState.IsValid)
            {
                _db.Posts.Update(post);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CategoryList = new SelectList(_db.Categories.ToList(), "Id", "Name");
            return View(post);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var post = await _db.Posts.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
            if (post == null) return NotFound();
            return View(post);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _db.Posts.FindAsync(id);
            _db.Posts.Remove(post);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}