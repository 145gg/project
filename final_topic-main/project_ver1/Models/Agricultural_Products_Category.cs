using System;
using System.Collections.Generic;

namespace project_ver1.Models;

public partial class Agricultural_Products_Category
{
    public int ID { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Agricultural_Products> AgriculturalProducts { get; set; } = new List<Agricultural_Products>();
}
