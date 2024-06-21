using System;
using System.Collections.Generic;

namespace project_ver1.Models;

public partial class Agricultural_Order
{
    public int ID { get; set; }

    public int CustomerID { get; set; }

    public DateTime OderTime { get; set; }

    public bool OrderFinished { get; set; }

    public int? EmployeeID { get; set; }
    public int? SumPrice { get; set; }



    public virtual ICollection<Agricultural_Order_Details> Agricultural_Order_Details { get; set; } = new List<Agricultural_Order_Details>();

    public virtual Customer Customer { get; set; } = null!;

    public virtual Employee? Employee { get; set; }
}


