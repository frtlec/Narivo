using Narivo.Checkout.Core.Clients.Dtos.NarivoCatalogApiDtos;
using Narivo.Checkout.Core.Infastructure.Enums;

namespace Narivo.Checkout.Core.Business.Dtos.ResponseDtos;

public class GetCheckoutPageDto
{
    public OrderStatus Status { get; set; }
    public int MemberId { get; set; }
    public decimal TotalPrice { get; set; }

    public List<OrderItemDto> Items { get; set; }
    public MemberDto Member { get; set; }
}