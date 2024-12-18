﻿namespace Domain.Entites;

public class Product : BaseEntity
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Slug { get; set; }

    public string? Description { get; set; }
    
    public string? DescriptionDetail { get; set; }

    public string? Type { get; set; }

    public Guid? StaffId { get; set; }

    public int? StockQuantity { get; set; }
    
    public int? Sold { get; set; }

    public decimal? Price { get; set; }
    
    public decimal? OriginalPrice { get; set; }
    
    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    
    public virtual ICollection<Image> Images { get; set; } = new List<Image>();
    
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    
    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
    
    public virtual Fish? Fish { get; set; }

    public virtual Tank? Tank { get; set; }
    
    public virtual Staff? Staff { get; set; }
}