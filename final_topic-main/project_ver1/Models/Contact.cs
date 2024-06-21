using System;
using System.Collections.Generic;

namespace project_ver1.Models;

public partial class Contact
{
    public int ID { get; set; }

    public string QuestionCategory { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool Sex { get; set; }

    public string Phone { get; set; } = null!;

    public string QuestionContent { get; set; } = null!;

    public int? EmployeeId { get; set; }

    public DateTime? SetTime { get; set; }

    public virtual Employee? Employee { get; set; }
    public bool Finished { get; set; }
}
