using Microsoft.AspNetCore.Mvc;
using Narivo.Checkout.Core.Business.Dtos.RequestDtos;
using Narivo.Checkout.Core.Business.Dtos.ResponseDtos;
using Refit;

namespace Narivo.WebUI.HttpClients
{
    public interface ICheckoutApiClient
    {
        [Post("/api/Order/Create")]
        Task<ApiResponse<int>> CreateOrder([Body] CreateOrderRequestDto order);

        [Get("/api/Order/Get")]
        Task<ApiResponse<OrderDto>> Get([Query] int id);

        [Get("/api/Order/GetAllByMemberId")]
        Task<ApiResponse<List<OrderDto>>> GetAllByMemberId([Query] int memberId);


        [Post("/api/checkout/start")]
        Task<ApiResponse<bool>> Start([FromBody] CheckoutRequestDto checkoutRequestDto);
    }
}
