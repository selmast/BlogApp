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
        private readonly IWebHostEnvironment _env;

        public PostController(BlogDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        private void PopulateCategoryDropdown()
        {
            ViewBag.CategoryList = new SelectList(_db.Categories.ToList(), "Id", "Name");
        }

        private string SaveImage(IFormFile image)
        {
            var folder = Path.Combine(_env.WebRootPath, "images", "posts");
            Directory.CreateDirectory(folder); // creates it if it doesn't exist yet

            var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
            var filePath = Path.Combine(folder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                image.CopyTo(stream);
            }

            return "/images/posts/" + fileName;
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

            post.ViewCount++;
            await _db.SaveChangesAsync();


            return View(post);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Search(string term)
        {
            var posts = await _db.Posts
                .Include(p => p.Category)
                .Where(p => p.IsActive && (p.Title.Contains(term) || p.Content.Contains(term)))
                .OrderByDescending(p => p.PublishDate)
                .ToListAsync();

            ViewBag.SearchTerm = term;
            return View(posts);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Category(int id)
        {
            var category = await _db.Categories.FindAsync(id);
            if (category == null) return NotFound();

            var posts = await _db.Posts
                .Include(p => p.Category)
                .Where(p => p.IsActive && p.CategoryId == id)
                .OrderByDescending(p => p.PublishDate)
                .ToListAsync();

            ViewBag.CategoryName = category.Name;
            return View(posts);
        }

        [AllowAnonymous]
        public async Task<IActionResult> All()
        {
            var posts = await _db.Posts
                .Include(p => p.Category)
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.PublishDate)
                .ToListAsync();

            return View(posts);
        }


        public IActionResult Create()
        {
            PopulateCategoryDropdown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Post post, IFormFile? CoverImage)
        {
            if (ModelState.IsValid)
            {
                if (CoverImage != null && CoverImage.Length > 0)
                {
                    post.CoverImageUrl = SaveImage(CoverImage);
                }

                _db.Posts.Add(post);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateCategoryDropdown();
            return View(post);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var post = await _db.Posts.FindAsync(id);
            if (post == null) return NotFound();
            PopulateCategoryDropdown();
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Post post, IFormFile? CoverImage)
        {
            var existingPost = await _db.Posts.FindAsync(post.Id);
            if (existingPost == null) return NotFound();

            if (!ModelState.IsValid)
            {
                PopulateCategoryDropdown();
                return View(post);
            }

            existingPost.Title = post.Title;
            existingPost.Summary = post.Summary;
            existingPost.Content = post.Content;
            existingPost.CategoryId = post.CategoryId;
            existingPost.IsActive = post.IsActive;

            if (CoverImage != null && CoverImage.Length > 0)
            {
                existingPost.CoverImageUrl = SaveImage(CoverImage);
            }
            // else: existingPost.CoverImageUrl stays exactly as it was

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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