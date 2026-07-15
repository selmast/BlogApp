using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BlogApp.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }


        [Required]
        public string Content { get; set; }

        public DateTime CommentDate { get; set; } = DateTime.Now;

        public bool IsApproved { get; set; } = false;

        [ForeignKey("Post")]
        public int PostId { get; set; }

        [ValidateNever]
        public Post Post { get; set; }
    }
}