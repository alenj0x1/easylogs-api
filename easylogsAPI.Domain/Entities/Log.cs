using System;
using System.Collections.Generic;

namespace easylogsAPI.Domain.Entities;

public partial class Log
{
    public Guid LogId { get; set; }

    public string Message { get; set; } = null!;

    public string Trace { get; set; } = null!;

    public string? Exception { get; set; }

    public string? StackTrace { get; set; }

    public int Type { get; set; }

    public string DataJson { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual Logtype TypeNavigation { get; set; } = null!;
}
