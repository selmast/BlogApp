using BlogApp.Models;
using BlogApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly BlogDbContext _db;
        private readonly ITranslationService _translationService;

        public CommentController(BlogDbContext db, ITranslationService translationService)
        {
            _db = db;
            _translationService = translationService;
        }

        // GET: /Comment  -- admin view of all comments
        public async Task<IActionResult> Index()
        {
            var comments = await _db.Comments.Include(c => c.Post).ToListAsync();
            return View(comments);
        }


        // POST: /Comment/Create  -- visitor submits a comment from Post/Details
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.IsApproved = false;
                _db.Comments.Add(comment);
                await _db.SaveChangesAsync();
                TempData["CommentSuccess"] = "Your comment was submitted and is awaiting approval.";
            }
            else
            {
                var errors = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                TempData["CommentError"] = errors;
            }
            return RedirectToAction("Details", "Post", new { id = comment.PostId });
        }

        // POST: /Comment/Approve/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var comment = await _db.Comments.FindAsync(id);
            if (comment == null) return NotFound();

            comment.IsApproved = true;
            comment.ContentEn = await _translationService.TranslateAsync(comment.Content, "EN");
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Comment/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _db.Comments.Include(c => c.Post).FirstOrDefaultAsync(c => c.Id == id);
            if (comment == null) return NotFound();
            return View(comment);
        }

        // POST: /Comment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _db.Comments.FindAsync(id);
            _db.Comments.Remove(comment);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}