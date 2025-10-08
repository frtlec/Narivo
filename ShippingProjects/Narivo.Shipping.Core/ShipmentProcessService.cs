

using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Narivo.Shared.Dtos.KafkaMessages;
using Narivo.Shared.Exceptions;
using Narivo.Shared.Helpers;
using Narivo.Shared.Kafka;
using Narivo.Shipping.Core.Dtos;
using Narivo.Shipping.Core.Integration;
using Narivo.Shipping.Core.Integration.Dtos;

namespace Narivo.Shipping.Core;
public class ShipmentProcessService
{
    private readonly IFakeShippingCompanyApiRefitClient _shippingClient;
    private readonly ICheckoutApiRefitClient _checkoutApiClient;
    private readonly IMembershipApiRefitClient _membershipApiRefitClient;
    private readonly ShippingProcessDbContext _db;
    private readonly KafkaProducer _kafkaProducer;
    private readonly KafkaConfig _kafkaConfig;

    public ShipmentProcessService(IFakeShippingCompanyApiRefitClient shippingClient,
                                  ShippingProcessDbContext db,
                                  KafkaProducer kafkaProducer,
                                  IOptions<KafkaConfig> kafkaConfig,
                                  ICheckoutApiRefitClient checkoutApiClient,
                                  IMembershipApiRefitClient membershipApiRefitClient)
    {
        _shippingClient = shippingClient;
        _db = db;
        _kafkaProducer = kafkaProducer;
        _kafkaConfig = kafkaConfig.Value;
        _checkoutApiClient = checkoutApiClient;
        _membershipApiRefitClient = membershipApiRefitClient;
    }

    public async Task<string> CreateShipment(CreateShipmentRequestDto request)
    {
        var order = await RefitWrapper.ExecuteAsync(() => _checkoutApiClient.Get(request.OrderId));
        if (order.Status != CheckoutApiOrderStatus.Paid)
            throw new AppException("Sipariş kargo için uygun değil, adres bilgisi eksik");
        if (order.SelectedAddressId.HasValue == false)
            throw new AppException("Sipariş kargo için uygun değil, adres bilgisi eksik");
        var member = await RefitWrapper.ExecuteAsync(() => _membershipApiRefitClient.GetByIdAndAddressId(order.MemberId, order.SelectedAddressId.Value));

        CreateShipmentRequestDTO dto = new CreateShipmentRequestDTO()
        {
            DeliveryPhone = member.PhoneNumber,
            DeliveryTargetEmail = member.Email,
            DeliveryTargetAddress = member.Address,
            DeliveryTargetFullName = $"{member.Name} {member.SurName}",
            SenderCompany = "Narivo",
            SenderCompanyAdddress = "BlaBla sk. 10001/111 Ümraniye/İstanbul",
            SenderEmail = "info@narivo.com",
            SenderPhone = "2163828181",
            TrackingNumber = request.TrackingCode

        };
        var response = await _shippingClient.CreateShipmentAsync(dto);
        _db.Orders.Add(new Order
        {
            CustomerName = dto.DeliveryTargetFullName,
            CustomerEmail = dto.DeliveryTargetEmail,
            CustomerPhone = dto.DeliveryPhone,
            Address = dto.DeliveryTargetAddress,
            ShippingCode = response.TrackingNumber,
            Status = ShippingStatus.SentToCarrier,
            CreatedAt = DateTime.UtcNow
        });

        await _kafkaProducer.ProduceAsync<OrderShippedMessageDto>(_kafkaConfig.OrderShippedTopic, new OrderShippedMessageDto
        {
            OrderId = request.OrderId,
            TrackingId = request.TrackingCode,
            CorrelationId = request.CorrelationId
        });

        return response.TrackingNumber;
    }
}