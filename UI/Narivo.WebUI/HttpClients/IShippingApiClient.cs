using Narivo.Checkout.Core.Business.Dtos.RequestDtos;
using Narivo.Shipping.Core.Dtos;
using Refit;

namespace Narivo.WebUI.HttpClients;

public interface IShippingApiClient
{
    [Post("/api/shipping/create")]
    Task<ApiResponse<string>> CreateShipment([Body] CreateShipmentRequestDto order);
}
