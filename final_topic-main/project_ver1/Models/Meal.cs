using System;
using System.Collections.Generic;

namespace project_ver1.Models;

public partial class Meal
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Stock { get; set; }

    public virtual ICollection<MealOrderDetail> MealOrderDetails { get; set; } = new List<MealOrderDetail>();
}
