using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models
{
    public enum Status
    {
        [Display(Name = "Pending")]
        Pending,

        [Display(Name = "In Progress")]
        InProgress,

        [Display(Name = "Completed")]
        Completed

    }
}
