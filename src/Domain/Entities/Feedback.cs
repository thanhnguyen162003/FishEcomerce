using System;
using System.Collections.Generic;

namespace FishEcomerce.Infrastructure;

public partial class Feedback
{
    public Guid Id { get; set; }

    public Guid? ProductId { get; set; }

    public Guid? UserId { get; set; }

    public string? Content { get; set; }

    public int? Rate { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Customer? User { get; set; }
}
