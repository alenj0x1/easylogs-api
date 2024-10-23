using System;
using System.Collections.Generic;

namespace easylogsAPI.Domain.Entities;

public partial class Userapppermission
{
    public int UserAppPermissionId { get; set; }

    public Guid Userid { get; set; }

    public int PermissionId { get; set; }

    public DateTime GivenAt { get; set; }

    public virtual Permission Permission { get; set; } = null!;

    public virtual Userapp User { get; set; } = null!;
}
