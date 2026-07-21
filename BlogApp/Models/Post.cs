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
        [Display(Name = "Title (Turkish)")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Content (Turkish)")]
        public string Content { get; set; }

        [Display(Name = "Summary (Turkish)")]
        public string Summary { get; set; }

        [MaxLength(200)]
        [Display(Name = "Title (English)")]
        public string? TitleEn { get; set; }

        [Display(Name = "Content (English)")]
        public string? ContentEn { get; set; }

        [Display(Name = "Summary (English)")]
        public string? SummaryEn { get; set; }

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