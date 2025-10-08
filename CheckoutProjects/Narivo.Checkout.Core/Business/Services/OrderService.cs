
using Microsoft.EntityFrameworkCore;
using Narivo.Checkout.Core.Business.Dtos.RequestDtos;
using Narivo.Checkout.Core.Business.Dtos.ResponseDtos;
using Narivo.Checkout.Core.Clients.Dtos.NarivoCatalogApiDtos;
using Narivo.Checkout.Core.Clients.RefitClients;
using Narivo.Checkout.Core.Infastructure.Entites;
using Narivo.Checkout.Core.Infastructure.Persistence;
using Narivo.Shared.Exceptions;
using System.Net;

namespace Narivo.Checkout.Core.Business.Services
{
    public interface IOrderService
    {
        Task<int> Create(CreateOrderRequestDto createOrderRequestDto);
        Task<OrderDto> Get(int id);
        Task<List<OrderDto>> GetAllByMemberId(int memberId);
        Task SetShipmentTrackingId(int orderId, string trackingCode);

    }
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _appDbContext;
        private readonly ICatalogApiClient _catalogApiClient;
        private readonly IProductService _productService;

        public OrderService(AppDbContext appDbContext, ICatalogApiClient catalogApiClient, IProductService productService)
        {
            _appDbContext = appDbContext;
            _catalogApiClient = catalogApiClient;
            _productService = productService;
        }

        public async Task<int> Create(CreateOrderRequestDto createOrderRequestDto)
        {
            var order = await GenerateOrder(createOrderRequestDto);
            await _appDbContext.Orders.AddAsync(order);
            await _appDbContext.SaveChangesAsync();

            return order.Id;
        }
        public async Task<OrderDto> Get(int id)
        {
            var order = await _appDbContext.Orders.Include(f => f.Payment).Include(f => f.Items).FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
                throw new AppException("Sipariş bulunamadı", System.Net.HttpStatusCode.NotFound);
            return new OrderDto(order);
        }

        public async Task<List<OrderDto>> GetAllByMemberId(int memberId)
        {
            var orders = await _appDbContext.Orders.Include(f => f.Items).Where(o => o.MemberId == memberId).OrderByDescending(f => f.Id).ToListAsync();
            return orders.Select(o => new OrderDto(o)).ToList();
        }

        public async Task SetShipmentTrackingId(int orderId, string trackingCode)
        {
            var order = await _appDbContext.Orders.FirstOrDefaultAsync(f => f.Id == orderId) ?? throw new AppException($"{orderId},Sipariş bulunamadı", HttpStatusCode.NotFound);
            order.ShipmentTrackingCode = trackingCode;
            await _appDbContext.SaveChangesAsync();
        }


        //private methods
        private async Task<Order> GenerateOrder(CreateOrderRequestDto createOrderRequestDto)
        {

            var items = new List<OrderItem>();

            foreach (var item in createOrderRequestDto.Items)
            {
                ProductDto product = await _productService.GetProduct(item.ProductId);
                items.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    ProductName = product.Name,
                    ProductType = product.ProductType,
                    Quantity = item.Quantity,
                    Price = product.UnitPrice * item.Quantity,
                    Status = Infastructure.Enums.OrderItemStatus.Pending,
                });
            }


            return new Order
            {
                MemberId = createOrderRequestDto.MemberId,
                Status = Infastructure.Enums.OrderStatus.Pending,
                TotalPrice = items.Sum(i => i.Price),
                Items = items,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }



    }
}
