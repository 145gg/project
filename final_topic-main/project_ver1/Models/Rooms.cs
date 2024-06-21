using System;
using System.Collections.Generic;

namespace project_ver1.Models;

public partial class Rooms
{
    public int ID { get; set; }

    public int CategoryID { get; set; }

    public bool HasReserved { get; set; }

    public bool HasOccupied { get; set; }

    public bool CanReserve { get; set; }

    public bool CanOccupy { get; set; }

    public int? Price { get; set; }

    public int? Discount { get; set; }

    public virtual Room_Category Category { get; set; } = null!;

    public virtual ICollection<Room_Order_Details> RoomOrderDetails { get; set; } = new List<Room_Order_Details>();
}
