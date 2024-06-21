using System;
using System.Collections.Generic;

namespace project_ver1.Models;

public partial class MealOrder
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public DateTime OderTime { get; set; }

    public int LumpSum { get; set; }

    public bool OrderFinished { get; set; }

    public int? EmployeeId { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<MealOrderDetail> MealOrderDetails { get; set; } = new List<MealOrderDetail>();
}
