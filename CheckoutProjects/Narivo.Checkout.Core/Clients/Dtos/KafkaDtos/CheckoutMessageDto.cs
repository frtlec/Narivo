namespace Narivo.Checkout.Core.Clients.Dtos.KafkaDtos;
public class CheckoutMessageDto
{
    public int OrderId { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public int SelectedCardId { get; set; }
    public int SelectedAddressId { get; set; }
}
