using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogApp.Models;

namespace BlogApp.Controllers
{
    public class CommentController : Controller
    {
        private readonly BlogDbContext _db;

        public CommentController(BlogDbContext db)
        {
            _db = db;
        }

        // GET: /Comment  -- admin view of all comments
        public async Task<IActionResult> Index()
        {
            var comments = await _db.Comments.Include(c => c.Post).ToListAsync();
            return View(comments);
        }

        // POST: /Comment/Approve/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var comment = await _db.Comments.FindAsync(id);
            if (comment == null) return NotFound();

            comment.IsApproved = true;
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