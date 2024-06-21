using System;
using System.Collections.Generic;

namespace project_ver1.Models;

public partial class Room_Category
{
    public int CategoryID { get; set; }

    public string Name { get; set; } = null!;

    public int Count { get; set; }

    public int OccupyNumber { get; set; }

    public virtual ICollection<Rooms> Rooms { get; set; } = new List<Rooms>();
}
