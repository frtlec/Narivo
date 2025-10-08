using Narivo.Shared.Constants.Enums;

namespace Narivo.Shared.Dtos.KafkaMessages;
public class CheckoutInitialMessageDto
{
    public int OrderId { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public int SelectedCardId { get; set; }
    public int SelectedAddressId { get; set; }
    public int MaxRetryCount = 3;
    public string CorrelationId { get; set; }
}


public class CheckoutStartMessageDto
{
    public int OrderId { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public int SelectedCardId { get; set; }
    public int SelectedAddressId { get; set; }
    public string CorrelationId { get; set; }
}

public class CheckoutSuccessfulMessageDto
{
    public int OrderId { get; set; }
    public string CorrelationId { get; set; }
}
public class CheckoutFailMessageDto
{
    public int OrderId { get; set; }
    public string Reason { get; set; }
    public string CorrelationId { get; set; }
}

public class OrderShippedMessageDto
{
    public int OrderId { get; set; }
    public string TrackingId { get; set; }
    public string CorrelationId { get; set; }
}