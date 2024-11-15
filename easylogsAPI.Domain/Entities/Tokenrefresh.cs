using System;
using System.Collections.Generic;

namespace easylogsAPI.Domain.Entities;

public partial class Tokenrefresh
{
    public int TokenRefreshId { get; set; }

    public Guid UserAppId { get; set; }

    public string Ip { get; set; } = null!;

    public bool IsApiKey { get; set; }

    public string Value { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? Expiration { get; set; }

    public virtual ICollection<Tokenaccess> Tokenaccesses { get; set; } = new List<Tokenaccess>();

    public virtual Userapp UserApp { get; set; } = null!;
}
