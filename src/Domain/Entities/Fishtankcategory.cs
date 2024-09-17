using System;
using System.Collections.Generic;

namespace FishEcomerce.Entities;


public partial class Fishtankcategory
{
    public Guid Id { get; set; }

    public Guid? Fishtankid { get; set; }

    public string? Fishtankcategorytype { get; set; }

    public string? Fishtanklevel { get; set; }

    public virtual Fishtank? Fishtank { get; set; }
}
