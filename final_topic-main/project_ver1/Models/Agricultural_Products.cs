using System;
using System.Collections.Generic;

namespace project_ver1.Models;

public partial class Agricultural_Products
{
    public int ID { get; set; }

    public int CategoryID { get; set; }

    public int Stock { get; set; }

    public string Name { get; set; } = null!;

    public string? Image { get; set; }

    public int Price { get; set; }

    public int? Discount { get; set; }
    public string? Description { get; set; }
    public string? Features { get; set; }
    public string? Made { get; set; }

    public virtual ICollection<Agricultural_Order_Details> AgriculturalOrderDetails { get; set; } = new List<Agricultural_Order_Details>();

    public virtual Agricultural_Products_Category Category { get; set; } = null!;
}
