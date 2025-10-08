using FakeShippingCompanyApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FakeShippingCompanyApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ShippingController : ControllerBase
{
    private readonly FakeShippingCompanyDbContext _context;

    public ShippingController(FakeShippingCompanyDbContext context)
    {
        _context = context;
    }
    [HttpPost]
    public async Task<ActionResult<CreateShipmentResponseDTO>> CreateShipment([FromBody] CreateShipmentRequestDTO dto)
    {
        var shipment = new Shipment
        {
            SenderCompany = dto.SenderCompany,
            SenderCompanyAdddress = dto.SenderCompanyAdddress,
            SenderEmail = dto.SenderEmail,
            SenderPhone = dto.SenderPhone,
            DeliveryTargetAddress = dto.DeliveryTargetAddress,
            DeliveryTargetFullName = dto.DeliveryTargetFullName,
            DeliveryTargetEmail = dto.DeliveryTargetEmail,
            DeliveryPhone = dto.DeliveryPhone,
            TrackingNumber = dto.TrackingNumber,
            Status = ShipmentStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        _context.Shipments.Add(shipment);
        await _context.SaveChangesAsync();

        return Ok(new CreateShipmentResponseDTO { TrackingNumber = shipment.TrackingNumber });
    }

    [HttpGet("status/{trackingNumber}")]
    public async Task<ActionResult<ShipmentStatusResponseDTO>> GetStatus(string trackingNumber)
    {
        var shipment = await _context.Shipments
                                     .FirstOrDefaultAsync(s => s.TrackingNumber == trackingNumber);

        if (shipment == null)
            return NotFound("Shipment not found");

        return Ok(new ShipmentStatusResponseDTO
        {
            TrackingNumber = shipment.TrackingNumber,
            Status = shipment.Status
        });
    }

    private string GenerateTrackingNumber()
    {
        return $"TRK-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
    }
}
