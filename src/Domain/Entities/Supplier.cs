using System;
using System.Collections.Generic;

namespace FishEcomerce.Domain.Entities;

public partial class Supplier
{
    public Guid Id { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? CompanyName { get; set; }

    public string? AddressStore { get; set; }

    public string? Facebook { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
