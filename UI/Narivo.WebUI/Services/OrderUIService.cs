using Narivo.Checkout.Core.Business.Dtos.RequestDtos;
using Narivo.Checkout.Core.Business.Dtos.ResponseDtos;
using Narivo.Shared.Helpers;
using Narivo.WebUI.HttpClients;
using Narivo.WebUI.Models;

namespace Narivo.WebUI.Services
{
    public class OrderUIService
    {
        private readonly ICheckoutApiClient _checkoutApiClient;
        private readonly IShippingApiClient _shippingApiClient;

        public OrderUIService(ICheckoutApiClient checkoutApiClient, IShippingApiClient shippingApiClient)
        {
            _checkoutApiClient = checkoutApiClient;
            _shippingApiClient = shippingApiClient;
        }

        public async Task<int?> CreateOrder(int memberId, List<BasketItem> basketItems)
        {
            var request = new CreateOrderRequestDto
            {
                MemberId = memberId,
                Items = basketItems.ConvertAll(i => new CreateOrderRequestDto.Item
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                })
            };

            var response = await _checkoutApiClient.CreateOrder(request);
            if (response.IsSuccessStatusCode && response.Content != null)
                return response.Content;

            return null;
        }
        public async Task<int?> CreateOrder(CreateOrderRequestDto request)
        {
            var response = await _checkoutApiClient.CreateOrder(request);
            if (response.IsSuccessStatusCode && response.Content != null)
                return response.Content;
            return null;
        }

        public async Task<OrderDto> Get(int orderId)
        {
            var response = await _checkoutApiClient.Get(orderId);
            if (response.IsSuccessStatusCode && response.Content != null)
                return response.Content;
            throw new Exception("Order not found");
        }

        public async Task<List<OrderDto>> GetAllByMemberId(int memberId)
        {
            var response = await _checkoutApiClient.GetAllByMemberId(memberId);
            if (response.IsSuccessStatusCode && response.Content != null)
                return response.Content;
            throw new Exception("Order not found");
        }


        public async Task<string> UpdateShipmentCode(int orderId, string trackingCode)
        {

            var response = await _shippingApiClient.CreateShipment(new Shipping.Core.Dtos.CreateShipmentRequestDto
            {
                CorrelationId = Guid.NewGuid().ToString(),
                OrderId = orderId,
                TrackingCode = trackingCode
            });
                return response?.Content ?? string.Empty;
        }
    }
}
