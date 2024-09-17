using System;
using System.Collections.Generic;

namespace FishEcomerce.Domain.Entities;

public partial class FishTankCategory
{
    public Guid Id { get; set; }

    public Guid? FishTankId { get; set; }

    public string? FishTankCategoryType { get; set; }

    public string? FishTankLevel { get; set; }

    public virtual FishTank? FishTank { get; set; }
}
