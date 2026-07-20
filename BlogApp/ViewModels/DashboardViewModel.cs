using BlogApp.Models;

namespace BlogApp.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalPosts { get; set; }
        public int TotalCategories { get; set; }
        public int TotalComments { get; set; }
        public List<Post> RecentPosts { get; set; }
        public List<Post> TopPosts { get; set; }
    }
}