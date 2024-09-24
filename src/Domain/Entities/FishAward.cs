using System;
using System.Collections.Generic;

namespace FishEcomerce.Domain.Entities;

public partial class FishAward
{
    public Guid Id { get; set; }

    public Guid? FishProductId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual FishProduct? FishProduct { get; set; }
}
