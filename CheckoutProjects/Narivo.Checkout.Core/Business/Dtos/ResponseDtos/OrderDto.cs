using Narivo.Checkout.Core.Infastructure.Entites;
using Narivo.Checkout.Core.Infastructure.Enums;
using Narivo.Shared.Dtos;
using System.Text.Json.Serialization;

namespace Narivo.Checkout.Core.Business.Dtos.ResponseDtos;
public class OrderDto:DtoBase
{
    public OrderStatus Status { get; set; }
    public int MemberId { get; set; }
    public decimal TotalPrice { get; set; }
    public List<OrderItemDto> Items { get; set; }



    [JsonConstructor]
    public OrderDto()
    {
        
    }
    public OrderDto(Order order)
    {
        Status= order.Status;
        MemberId= order.MemberId;
        TotalPrice= order.TotalPrice;
        Items= order.Items.Select(i => new OrderItemDto(i)).ToList();
        Id= order.Id;
        CreatedAt = order.CreatedAt;
        UpdatedAt = order.UpdatedAt;

    }

}
