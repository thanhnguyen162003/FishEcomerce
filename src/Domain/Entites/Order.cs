namespace Domain.Entites;

public class Order : BaseEntity
{
    public Guid Id { get; set; }

    public Guid? CustomerId { get; set; }

    public DateTime? OrderDate { get; set; }

    public DateTime? ShippedDate { get; set; }

    public decimal? TotalPrice { get; set; }
    
    public string? Status { get; set; }
    
    public int? OrderCode { get; set; }
    
    public string? PaymentMethod { get; set; }

    public string? ShipAddress { get; set; }
    
    public virtual Customer? Customer { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}