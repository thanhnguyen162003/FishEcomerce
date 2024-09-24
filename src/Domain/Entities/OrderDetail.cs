using System;
using System.Collections.Generic;

namespace FishEcomerce.Infrastructure;

public partial class OrderDetail
{
    public Guid Id { get; set; }

    public Guid? OrderId { get; set; }

    public Guid? ProductId { get; set; }

    public int? Quantity { get; set; }

    public decimal? UnitPrice { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }
}
