using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Name (Turkish)")]
        public string Name { get; set; }

        [MaxLength(500)]
        [Display(Name = "Description (Turkish)")]
        public string Description { get; set; }

        [MaxLength(100)]
        [Display(Name = "Name (English)")]
        public string? NameEn { get; set; }

        [MaxLength(500)]
        [Display(Name = "Description (English)")]
        public string? DescriptionEn { get; set; }
    }
}