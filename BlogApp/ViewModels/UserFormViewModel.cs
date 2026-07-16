using System.ComponentModel.DataAnnotations;

namespace BlogApp.ViewModels
{
    public class UserFormViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }

        
        public string Password { get; set; }
    }
}