using System;
using System.Collections.Generic;

namespace FishEcomerce.Domain.Entities;

public partial class Feedback
{
    public Guid Id { get; set; }

    public Guid? ProductId { get; set; }

    public Guid? UserId { get; set; }

    public string? Content { get; set; }

    public int? Rate { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Customer? User { get; set; }
}
