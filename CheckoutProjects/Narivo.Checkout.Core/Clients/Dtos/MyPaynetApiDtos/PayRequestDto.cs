namespace Narivo.Checkout.Core.Business.Dtos.MyPaynetAPiDtos;

public class PayRequestDto
{
    public int UniqueId { get; set; }
    public decimal Total { get; set; }
    public MyPaynetApiCardInfo CardInfo { get; set; }
}
