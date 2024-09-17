using System;
using System.Collections.Generic;
namespace FishEcomerce.Entities;


public partial class Feedback
{
    public Guid Id { get; set; }

    public Guid? Productid { get; set; }

    public Guid? Userid { get; set; }

    public string? Content { get; set; }

    public int? Rate { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Customer? User { get; set; }
}
