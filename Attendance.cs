using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EmployeeAttendance.Models
{
    public class Attendance
    {
        [Key]
        public int AttendanceId { get; set; }

        [Required(ErrorMessage = "Employee ID is required")]
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public virtual required Employee Employee { get; set; } // Navigation property

        [Required(ErrorMessage = "Date is required")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Check-in Time is required")]
        [DataType(DataType.Time)]
        public TimeSpan CheckInTime { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan? CheckOutTime { get; set; }
    }
}
