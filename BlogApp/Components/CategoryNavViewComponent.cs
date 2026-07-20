using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Components
{
    public class CategoryNavViewComponent : ViewComponent
    {
        private readonly BlogDbContext _db;

        public CategoryNavViewComponent(BlogDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _db.Categories.ToListAsync();
            return View(categories);
        }
    }
}