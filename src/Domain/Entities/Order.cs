using System;
using System.Collections.Generic;

namespace FishEcomerce.Infrastructure;

public partial class Order
{
    public Guid Id { get; set; }

    public Guid? CustomerId { get; set; }

    public DateTime? OrderDate { get; set; }

    public DateTime? ShippedDate { get; set; }

    public decimal? TotalPrice { get; set; }

    public string? ShipAddress { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
