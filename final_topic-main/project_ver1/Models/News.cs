using System;
using System.Collections.Generic;

namespace project_ver1.Models;

public partial class News
{
    public int ID { get; set; }

    public string NewsTitle { get; set; } = null!;

    public string NewsContent { get; set; } = null!;

    public DateTime? SetTime { get; set; }

    public int? CategoryID { get; set; }

    public string? Image { get; set; }
}
