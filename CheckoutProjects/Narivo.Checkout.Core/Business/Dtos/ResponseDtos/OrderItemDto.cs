using Narivo.Checkout.Core.Infastructure.Entites;
using System.Text.Json.Serialization;

namespace Narivo.Checkout.Core.Business.Dtos.ResponseDtos;

public class OrderItemDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }

    [JsonConstructor]
    public OrderItemDto()
    {

    }
    public OrderItemDto(OrderItem orderItem)
    {
        ProductId= orderItem.ProductId;
        Quantity= orderItem.Quantity;
        Price= orderItem.Price;
        ProductName= orderItem.ProductName;
    }
}
