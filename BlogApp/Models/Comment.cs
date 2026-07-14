using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CommentDate { get; set; } = DateTime.Now;

        public bool IsApproved { get; set; } = false;

        [ForeignKey("Post")]
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}