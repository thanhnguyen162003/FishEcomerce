using Application.Common.Models.FishModels;
using Domain.Entites;

namespace Application.Common.Models.ProductModels;

public class ProductResponseModel
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Slug { get; set; }

    public string? Description { get; set; }

    public string? DescriptionDetail { get; set; }

    public string? Type { get; set; }

    public Guid? SupplierId { get; set; }

    public int? StockQuantity { get; set; }

    public int? Sold { get; set; }

    public decimal? Price { get; set; }

    public decimal? OriginalPrice { get; set; }

    //public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    //public virtual ICollection<Image> Images { get; set; } = new List<Image>();
    public virtual FishResponseModel? Fish { get; set; }

}