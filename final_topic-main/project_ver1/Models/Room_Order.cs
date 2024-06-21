using System;
using System.Collections.Generic;

namespace project_ver1.Models;

public partial class Room_Order
{
    public int ID { get; set; }

    public int CustomerID { get; set; }

    public DateTime OrderTime { get; set; }

    public DateTime CheckIn { get; set; }

    public DateTime CheckOut { get; set; }

    public bool OrderFinished { get; set; }

    public int? EmployeeID { get; set; }

    public int? SumPrice { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<Room_Order_Details> RoomOrderDetails { get; set; } = new List<Room_Order_Details>();
}
