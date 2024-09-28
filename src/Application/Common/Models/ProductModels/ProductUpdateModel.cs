﻿namespace Application.Common.Models.ProductModels;

public class ProductUpdateModel
{
    public string? Name { get; set; }
    
    public string? Description { get; set; }
    
    public string? DescriptionDetail { get; set; }
    
    public int? StockQuantity { get; set; }
    
    public decimal? Price { get; set; }
    
    public decimal? OriginalPrice { get; set; }
}