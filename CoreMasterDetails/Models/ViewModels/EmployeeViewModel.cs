using System.ComponentModel.DataAnnotations;

namespace CoreMasterDetails.Models.ViewModels
{
    public class EmployeeViewModel
    {
        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; } = null!;
        [Required, Display(Name = "Date of Birth"), DataType(DataType.Date)]
        public DateTime JoiningDate { get; set; } = DateTime.Now;

        public string MobileNo { get; set; }

        public string? ImageUrl { get; set; }

        public bool IsDeleted { get; set; }

        public int ReviewId { get; set; }
        public string ReviewNotes { get; set; }

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public int Budget { get; set; }


        public virtual PerformanceReview PerformanceReview { get; set; }
        public List<PerformanceReview> PerformanceReviews { get; set; }


        public virtual IList<Department> Departments { get; set; } = new List<Department>();

        public IList<Employee> Employees { get; set; }
        public IFormFile ProfileFile { get; set; }

        
    }
}
