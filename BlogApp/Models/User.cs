using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }

        [Required]
        public string PasswordHash { get; set; }
    }
}