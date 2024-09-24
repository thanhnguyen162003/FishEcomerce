using System;
using System.Collections.Generic;

namespace FishEcomerce.Domain.Entities;

public partial class Image
{
    public Guid Id { get; set; }

    public string? CloudLink { get; set; }

    public Guid? ProductId { get; set; }

    public Guid? BlogId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Blog? Blog { get; set; }

    public virtual Product? Product { get; set; }
}
