using System;
using System.Collections.Generic;

namespace project_ver1.Models;

public partial class Agricultural_Order_Details
{
    public int OrderID { get; set; }

    public int ProductID { get; set; }

    public int Count { get; set; }

    public int? Price { get; set; }

    public virtual Agricultural_Order Order { get; set; } = null!;

    public virtual Agricultural_Products Product { get; set; } = null!;
}
