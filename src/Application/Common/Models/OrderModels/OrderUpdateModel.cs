using Domain.Constants;
using Domain.Enums;

namespace Application.Common.Models.OrderModels;

public class OrderUpdateModel
{
    public bool? IsPaid { get; set; }
    public DateTime? ShippedDate { get; set; }
    public OrderStatus? OrderStatus { get; set; }
}