using System;
using System.Collections.Generic;

namespace CoreMasterDetails.Models;

public partial class PerformanceReview
{
    public int ReviewId { get; set; }

    public string ReviewNotes { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
