﻿using Application.Common.Models.CategoryModels;
using Application.Common.Models.FeedbackModels;
using Application.Common.Models.FishModels;
using Application.Common.Models.ImageModels;
using Application.Common.Models.TankModels;
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

    public Guid? StaffId { get; set; }

    public int? StockQuantity { get; set; }

    public int? Sold { get; set; }

    public decimal? Price { get; set; }

    public decimal? OriginalPrice { get; set; }

    public ICollection<FeedbackResponseModel> Feedbacks { get; set; } = new List<FeedbackResponseModel>();

    public IEnumerable<ImageResponseModel> Images { get; set; } = new List<ImageResponseModel>();
    public FishResponseModel? Fish { get; set; }
    
    public TankResponseModel? Tank { get; set; }
    
    public IEnumerable<CategoryResponseModel> Categories { get; set; } = new List<CategoryResponseModel>();

}