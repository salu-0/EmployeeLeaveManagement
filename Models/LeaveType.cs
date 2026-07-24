using System.ComponentModel.DataAnnotations;

namespace EmployeeLeaveManagement.Models
{
    public class LeaveType
    {
        [Key]
        public int LeaveTypeId { get; set; }

        [Required(ErrorMessage = "Leave type name is required.")]
        [Display(Name = "Leave Type")]
        [StringLength(100)]
        public string LeaveName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Allowed Days")]
        [Range(1, 365)]
        public int DefaultDays { get; set; }

        [StringLength(250)]
        public string? Description { get; set; }

        [Display(Name = "Status")]
        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
