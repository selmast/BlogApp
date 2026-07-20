using BlogApp.Models;
using BlogApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly BlogDbContext _db;

        public HomeController(BlogDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var lastCommentPerPost = await _db.Comments
                .Where(c => c.IsApproved)
                .GroupBy(c => c.PostId)
                .Select(g => new { PostId = g.Key, LastCommentDate = g.Max(c => c.CommentDate) })
                .OrderByDescending(x => x.LastCommentDate)
                .Take(5)
                .ToListAsync();

            var discussedPostIds = lastCommentPerPost.Select(x => x.PostId).ToList();

            var unorderedDiscussedPosts = await _db.Posts
                .Include(p => p.Category)
                .Where(p => discussedPostIds.Contains(p.Id))
                .ToListAsync();

            var recentlyDiscussedPosts = discussedPostIds
                .Select(id => unorderedDiscussedPosts.First(p => p.Id == id))
                .ToList();

            var model = new HomeViewModel
            {
                RecentPosts = await _db.Posts
                    .Include(p => p.Category)
                    .Where(p => p.IsActive)
                    .OrderByDescending(p => p.PublishDate)
                    .Take(3)
                    .ToListAsync(),

                MostReadPosts = await _db.Posts
                    .Include(p => p.Category)
                    .Where(p => p.IsActive)
                    .OrderByDescending(p => p.ViewCount)
                    .Take(3)
                    .ToListAsync(),

                Categories = await _db.Categories.ToListAsync(),

                RecentlyDiscussedPosts = recentlyDiscussedPosts
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}