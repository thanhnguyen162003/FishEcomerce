using System;
using System.Collections.Generic;

namespace FishEcomerce.Domain.Entities;

public partial class Breed
{
    public Guid Id { get; set; }

    public Guid? FishProductId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual FishProduct? FishProduct { get; set; }
}
