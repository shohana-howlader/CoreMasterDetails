using System;
using System.Collections.Generic;

namespace CoreMasterDetails.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string EmployeeName { get; set; } = null!;

    public DateTime JoiningDate { get; set; }

    public string MobileNo { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public bool IsDeleted { get; set; }

    public int ReviewId { get; set; }

    public virtual PerformanceReview PerformanceReview { get; set; } = null!;

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();
}
