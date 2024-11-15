using System;
using System.Collections.Generic;

namespace easylogsAPI.Domain.Entities;

public partial class Userapp
{
    public Guid UserAppId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int SessionTypeId { get; set; }

    public int SessionTime { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Sessiontype SessionType { get; set; } = null!;

    public virtual ICollection<Tokenaccess> Tokenaccesses { get; set; } = new List<Tokenaccess>();

    public virtual ICollection<Tokenrefresh> Tokenrefreshes { get; set; } = new List<Tokenrefresh>();

    public virtual ICollection<Userapppermission> Userapppermissions { get; set; } = new List<Userapppermission>();
}
