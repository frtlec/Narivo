namespace Narivo.Checkout.Core.Business.Dtos.MyPaynetAPiDtos;

public class MyPaynetApiCardInfo
{
    public string CardNumber { get; set; }
    public string CardHolderName { get; set; }
    public string Year { get; set; }
    public string Month { get; set; }
    public string CVV { get; set; }
    public string Bank { get; set; }
}
