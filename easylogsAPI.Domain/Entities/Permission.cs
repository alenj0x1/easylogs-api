using System;
using System.Collections.Generic;

namespace easylogsAPI.Domain.Entities;

public partial class Permission
{
    public int PermissionId { get; set; }
    public string Name { get; set; } = null!;
    public string ShowName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public virtual ICollection<Userapppermission> Userapppermissions { get; set; } = new List<Userapppermission>();
}
