using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Narivo.Checkout.Core.Business.Dtos.RequestDtos;
using Narivo.Checkout.Core.Business.Services;

namespace Narivo.Checkout.API.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequestDto dto)
    {
        var result = await _orderService.Create(dto);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllByMemberId([FromQuery] int memberId)
    {
        var result = await _orderService.GetAllByMemberId(memberId);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int id)
    {
        var result = await _orderService.Get(id);
        return Ok(result);
    }
}