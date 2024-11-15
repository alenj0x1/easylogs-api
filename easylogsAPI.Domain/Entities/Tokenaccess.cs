using System;
using System.Collections.Generic;

namespace easylogsAPI.Domain.Entities;

public partial class Tokenaccess
{
    public int TokenAccessId { get; set; }

    public int TokenRefreshId { get; set; }

    public Guid UserAppId { get; set; }

    public string Ip { get; set; } = null!;

    public bool IsApiKey { get; set; }

    public string Value { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime Expiration { get; set; }

    public virtual Tokenrefresh TokenRefresh { get; set; } = null!;

    public virtual Userapp UserApp { get; set; } = null!;
}
