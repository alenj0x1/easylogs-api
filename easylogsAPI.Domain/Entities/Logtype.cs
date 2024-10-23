using System;
using System.Collections.Generic;

namespace easylogsAPI.Domain.Entities;

public partial class Logtype
{
    public int LogTypeId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();
}
