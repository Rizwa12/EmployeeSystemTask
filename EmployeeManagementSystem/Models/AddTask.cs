using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models
{
   public class AddTask
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        public string? PicturePath { get; set; }

        [NotMapped]
        public IFormFile? PictureFile { get; set; }

        [Display(Name = "Assign To (Employee)")]
        public int? AssigneeId { get; set; }
        //public User? Assignee { get; set; }

        [Required]
        public Status Status { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Reward must be a positive value")]
        public decimal RewardPrice { get; set; }
        public int EmployerId { get; set; }

        public string? EmployeeComments { get; set; }

    }
}
