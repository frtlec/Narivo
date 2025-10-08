using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Narivo.Checkout.Core.Business.Dtos.MyPaynetAPiDtos;
using Narivo.Checkout.Core.Business.Dtos.RequestDtos;
using Narivo.Checkout.Core.Business.Dtos.ResponseDtos;
using Narivo.Checkout.Core.Clients.RefitClients;
using Narivo.Checkout.Core.Infastructure.Enums;
using Narivo.Checkout.Core.Infastructure.Hubs;
using Narivo.Checkout.Core.Infastructure.Persistence;
using Narivo.Shared.Constants.Enums;
using Narivo.Shared.Dtos.KafkaMessages;
using Narivo.Shared.Exceptions;
using Narivo.Shared.Helpers;
using Narivo.Shared.Kafka;

namespace Narivo.Checkout.Core.Business.Services;

public interface ICheckoutService
{
    Task Start(CheckoutRequestDto dto);
    Task Checkout(CheckoutStartMessageDto checkoutMessageDto);
    Task SendResultToClient(CheckoutSendResultToClient resultToClient);

}
public class CheckoutService : ICheckoutService
{
    private readonly AppDbContext _dbContext;
    private readonly KafkaProducer _kafkaProducer;
    private readonly KafkaConfig _kafkaConfig;
    private readonly IMyPayNetApiClient _myPayNetApiClient;
    private readonly IMembershipApiClient _memberApiClient;
    private readonly IHubContext<CheckoutHub> _hubContext;
    public CheckoutService(AppDbContext dbContext,
                           KafkaProducer kafkaProducer,
                           IOptions<KafkaConfig> kafkaConfig,
                           IMyPayNetApiClient myPayNetApiClient,
                           IMembershipApiClient memberApiClient,
                           IProductService productService,
                           IHubContext<CheckoutHub> hubContext)
    {
        _dbContext = dbContext;
        _kafkaProducer = kafkaProducer;
        _kafkaConfig = kafkaConfig.Value;
        _myPayNetApiClient = myPayNetApiClient;
        _memberApiClient = memberApiClient;
        _hubContext = hubContext;
    }
    public async Task Start(CheckoutRequestDto dto)
    {
        var order = await _dbContext.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == dto.OrderId) ?? throw new AppException("Sipariş bulunamadı", System.Net.HttpStatusCode.NotFound);

        var kafkaMessage = new CheckoutStartMessageDto
        {
            OrderId = dto.OrderId,
            Email = dto.Email,
            Phone = dto.Phone,
            SelectedCardId = dto.SelectedCardId,
            SelectedAddressId = dto.SelectedAddressId,
            CorrelationId = dto.CorrelationId
        };
        await _kafkaProducer.ProduceAsync<CheckoutStartMessageDto>(_kafkaConfig.CheckoutInitialTopic, kafkaMessage);

    }

    public async Task Checkout(CheckoutStartMessageDto checkoutMessageDto)
    {
        var order = await _dbContext.Orders.Include(f => f.Items).FirstOrDefaultAsync(f => f.Id == checkoutMessageDto.OrderId);

        if (order == null)
            throw new Exception("Sipariş bulunamadı");


        var card = await RefitWrapper.ExecuteAsync(() => _memberApiClient.GetCard(order.MemberId, checkoutMessageDto.SelectedCardId));



        var response = await _myPayNetApiClient.Create(new PayRequestDto
        {
            CardInfo = new MyPaynetApiCardInfo
            {
                Bank = card.Bank.ToString(),
                CardHolderName = card.HolderName,
                CardNumber = card.No,
                CVV = card.CVV,
                Month = card.Month.ToString(),
                Year = card.Year.ToString()
            },
            UniqueId = order.Id,
            Total = order.TotalPrice
        });

        if (response.IsSuccessStatusCode == false)
        {
            var paymentError = await _dbContext.Payments.FirstOrDefaultAsync(p => p.OrderId == order.Id);
            if (paymentError == null)
            {
                paymentError = new Infastructure.Entites.Payment
                {
                    OrderId = order.Id,
                    IsSuccess = false,
                    ErrorMessage = response.Error.Message,
                    CreatedAt = DateTime.UtcNow
                };
                await _dbContext.Payments.AddAsync(paymentError);
                await _dbContext.SaveChangesAsync();
            }

            order.PaymentId = paymentError.Id;
            order.Status = OrderStatus.Pending;
            await _dbContext.SaveChangesAsync();

            await _kafkaProducer.ProduceAsync<CheckoutFailMessageDto>(_kafkaConfig.CheckoutFailTopic, new CheckoutFailMessageDto
            {
                OrderId = order.Id,
                Reason = response.Error.Message,
                CorrelationId = checkoutMessageDto.CorrelationId,
            });

            throw new AppException($"Ödeme yapılamadı,{response.Error.Message}", response.StatusCode);
        }

        var payment = new Infastructure.Entites.Payment
        {
            OrderId = order.Id,
            IsSuccess = true,
            ErrorMessage = string.Empty,
            CreatedAt = DateTime.UtcNow,
            TransactionId = response.Content?.TransactionId.ToString(),
            CardId = checkoutMessageDto.SelectedCardId
        };

        await _dbContext.Payments.AddAsync(payment);
        await _dbContext.SaveChangesAsync();

        order.PaymentId = payment.Id;
        order.Status = OrderStatus.Paid;
        order.SelectedAddressId = checkoutMessageDto.SelectedAddressId;
        order.Items.ToList().ForEach(i => i.Status = OrderItemStatus.Paid);
        await _dbContext.SaveChangesAsync();

        await _kafkaProducer.ProduceAsync<CheckoutSuccessfulMessageDto>(_kafkaConfig.CheckoutSuccessfulTopic, new CheckoutSuccessfulMessageDto
        {
            OrderId = order.Id,
            CorrelationId = checkoutMessageDto.CorrelationId
        });
        return;

    }

    public async Task SendResultToClient(CheckoutSendResultToClient resultToClient)
    {
        await _hubContext.Clients.Group(resultToClient.CorrelationId)
             .SendAsync("ReceiveCheckoutResult", resultToClient);
    }
}
