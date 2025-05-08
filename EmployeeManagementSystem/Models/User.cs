using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Phone { get; set; }

        [Required]
        public string? Address { get; set; }

        public string? PicturePath { get; set; }

        [Required]
        public string? Role { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [NotMapped] 
        public IFormFile? PictureFile { get; set; }
    }
}
