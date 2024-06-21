using System;
using System.Collections.Generic;

namespace project_ver1.Models;

public partial class MealOrderDetail
{
    public int OrderId { get; set; }

    public int MealId { get; set; }

    public int Count { get; set; }

    public int Price { get; set; }

    public bool Finished { get; set; }

    public virtual Meal Meal { get; set; } = null!;

    public virtual MealOrder Order { get; set; } = null!;
}
