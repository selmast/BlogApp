using BlogApp.Models;
using BlogApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class DashboardController : Controller
{
    private readonly BlogDbContext _db;

    public DashboardController(BlogDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var model = new DashboardViewModel
        {
            TotalPosts = await _db.Posts.CountAsync(),
            TotalCategories = await _db.Categories.CountAsync(),
            TotalComments = await _db.Comments.CountAsync(),
            RecentPosts = await _db.Posts
                .Include(p => p.Category)
                .OrderByDescending(p => p.PublishDate)
                .Take(5)
                .ToListAsync(),
            TopPosts = await _db.Posts
                .Include(p => p.Category)
                .OrderByDescending(p => p.ViewCount)
                .Take(5)
                .ToListAsync()
        };

        return View(model);
    }
}