
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Narivo.Shipping.Core;
using Narivo.Shipping.Core.Dtos;
using Narivo.Shipping.Core.Integration;

namespace Narivo.ShippingApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ShippingController : ControllerBase
{

    private readonly ShipmentProcessService shipmentProcessService;

    public ShippingController(ShipmentProcessService shipmentProcessService)
    {
        this.shipmentProcessService = shipmentProcessService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateShipmentRequestDto request)
    {
        var result = await this.shipmentProcessService.CreateShipment(request);
        return Ok(result);
    }
}
