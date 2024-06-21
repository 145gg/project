using System;
using System.Collections.Generic;

namespace project_ver1.Models;

public partial class Employee
{
    public int ID { get; set; }

    public string Name { get; set; } = null!;

    public DateTime BirthDate { get; set; }

    public DateTime HireDate { get; set; }

    public string Address { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Pwd { get; set; }

    public string Department { get; set; } = null!;

    public virtual ICollection<Agricultural_Order> AgriculturalOrders { get; set; } = new List<Agricultural_Order>();

    public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();

    public virtual ICollection<MealOrder> MealOrders { get; set; } = new List<MealOrder>();

    public virtual ICollection<Room_Order> RoomOrders { get; set; } = new List<Room_Order>();
}
