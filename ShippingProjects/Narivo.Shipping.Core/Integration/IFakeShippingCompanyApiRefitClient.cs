using Refit;

namespace Narivo.Shipping.Core.Integration;

public interface IFakeShippingCompanyApiRefitClient
{
    [Post("/api/shipping/createshipment")]
    Task<CreateShipmentResponseDTO> CreateShipmentAsync([Body] CreateShipmentRequestDTO shipment);

    [Get("/api/shipping/status/{trackingNumber}")]
    Task<ShipmentStatusResponseDTO> GetShipmentStatusAsync(string trackingNumber);
}

public class CreateShipmentRequestDTO
{
    public string SenderCompany { get; set; }
    public string SenderCompanyAdddress { get; set; }
    public string SenderEmail { get; set; }
    public string SenderPhone { get; set; }
    public string DeliveryTargetAddress { get; set; }
    public string DeliveryTargetFullName { get; set; }
    public string DeliveryTargetEmail { get; set; }
    public string DeliveryPhone { get; set; }
    public string TrackingNumber { get; set; }
}

public class CreateShipmentResponseDTO
{
    public string TrackingNumber { get; set; }
}

public class ShipmentStatusResponseDTO
{
    public string TrackingNumber { get; set; }
    public ShipmentStatus Status { get; set; }
}

public enum ShipmentStatus
{
    Pending,
    PickedUp,
    InTransit,
    Delivered,
    Cancelled
}