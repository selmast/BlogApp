using BlogApp.Models;

namespace BlogApp.ViewModels
{
    public class HomeViewModel
    {
        public List<Post> RecentPosts { get; set; }
        public List<Post> MostReadPosts { get; set; }
        public List<Category> Categories { get; set; }
        public string SearchTerm { get; set; }
    }
}