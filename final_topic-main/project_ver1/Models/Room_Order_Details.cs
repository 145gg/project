using System;
using System.Collections.Generic;

namespace project_ver1.Models;

public partial class Room_Order_Details
{
    public int OrderID { get; set; }

    public int RoomID { get; set; }


    public int? Price { get; set; }
    public virtual Room_Order Order { get; set; } = null!;

    public virtual Rooms Room { get; set; } = null!;

}
