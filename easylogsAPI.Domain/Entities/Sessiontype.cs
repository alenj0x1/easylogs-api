using System;
using System.Collections.Generic;

namespace easylogsAPI.Domain.Entities;

public partial class Sessiontype
{
    public int SessionTypeId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Userapp> Userapps { get; set; } = new List<Userapp>();
}
