using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Narivo.Checkout.Core.Business.Dtos.RequestDtos;
using Narivo.Checkout.Core.Business.Services;
using Narivo.Checkout.Core.Infastructure.Hubs;

namespace Narivo.Checkout.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;
        private readonly IHubContext<SimpleHub> _hubContext;
        public CheckoutController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }
        [HttpPost]
        public async Task<IActionResult> Start([FromBody] CheckoutRequestDto checkoutRequestDto)
        {
            await _checkoutService.Start(checkoutRequestDto);
            return Ok(true);
        }

        [HttpPost]
        public async Task<IActionResult> SendResultToClient([FromBody] CheckoutSendResultToClient checkoutSend)
        {
            await _checkoutService.SendResultToClient(checkoutSend);
            return Ok(true);
        }
    }
}
