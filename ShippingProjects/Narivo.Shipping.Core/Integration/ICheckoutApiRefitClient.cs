using Narivo.Shipping.Core.Integration.Dtos;
using Refit;

namespace Narivo.Shipping.Core.Integration;
public interface ICheckoutApiRefitClient
{
    [Get("/api/order/get")]
    public Task<ApiResponse<CheckoutApiOrderDto>> Get([Query] int id);
}
