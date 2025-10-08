using System.ComponentModel.DataAnnotations;

namespace FakeShippingCompanyApi.Controllers;

public class CreateShipmentRequestDTO
{
    [Required, StringLength(150)]
    public string SenderCompany { get; set; }

    [Required, StringLength(2000)]
    public string SenderCompanyAdddress { get; set; }

    [Required, StringLength(2000)]
    public string SenderEmail { get; set; }

    [Required, StringLength(11)]
    public string SenderPhone { get; set; }

    [Required, StringLength(2000)]
    public string DeliveryTargetAddress { get; set; }

    [Required, StringLength(150)]
    public string DeliveryTargetFullName { get; set; }

    [Required, StringLength(100)]
    public string DeliveryTargetEmail { get; set; }

    [Required, StringLength(13)]
    public string DeliveryPhone { get; set; }
    public string TrackingNumber { get; set; } = string.Empty;
}
