using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BlogApp.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public string Summary { get; set; }

        [ValidateNever]
        public string CoverImageUrl { get; set; }

        public DateTime PublishDate { get; set; } = DateTime.Now;

        public int ViewCount { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        [ForeignKey("Category")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [ValidateNever]
        public Category Category { get; set; }

        [ValidateNever]
        public ICollection<Comment> Comments { get; set; }
    }
}