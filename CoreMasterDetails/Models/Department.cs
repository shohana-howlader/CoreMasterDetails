using System;
using System.Collections.Generic;

namespace CoreMasterDetails.Models;

public partial class Department
{
    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; } = null!;

    public int Budget { get; set; }

    public int EmployeeId { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
