using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Narivo.Checkout.Core.Business.Dtos.RequestDtos;
using Narivo.Checkout.Core.Business.Services;

namespace Narivo.Checkout.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;
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

    }
}
