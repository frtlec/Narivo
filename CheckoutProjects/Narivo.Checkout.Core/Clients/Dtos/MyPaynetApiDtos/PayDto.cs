namespace Narivo.Checkout.Core.Business.Dtos.MyPaynetAPiDtos;

public class PayDto
{
    public Guid TransactionId { get; set; }
    public MyPayNetApiPaymentStatus PayStatus { get; set; }
}
