using System;
using System.Collections.Generic;

namespace FishEcomerce.Domain.Entities;

public partial class FishTankFishTankCategory
{
    public Guid Id { get; set; }

    public Guid? FishTankId { get; set; }

    public Guid? FishTankCategoryId { get; set; }

    public virtual FishTank? FishTank { get; set; }

    public virtual FishTankCategory? FishTankCategory { get; set; }
}
