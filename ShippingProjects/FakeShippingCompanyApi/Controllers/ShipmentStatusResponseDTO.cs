using FakeShippingCompanyApi.Data;

namespace FakeShippingCompanyApi.Controllers;

public class ShipmentStatusResponseDTO
{
    public string TrackingNumber { get; set; }
    public ShipmentStatus Status { get; set; }
}