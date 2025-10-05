using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Narivo.Checkout.Core.Business.Dtos.MyPaynetAPiDtos;
using Narivo.Checkout.Core.Business.Dtos.RequestDtos;
using Narivo.Checkout.Core.Business.Dtos.ResponseDtos;
using Narivo.Checkout.Core.Clients.Dtos.KafkaDtos;
using Narivo.Checkout.Core.Clients.RefitClients;
using Narivo.Checkout.Core.Infastructure.Persistence;
using Narivo.Shared.Helpers;
using Narivo.Shared.Kafka;

namespace Narivo.Checkout.Core.Business.Services;

public interface ICheckoutService
{
    Task Start(CheckoutRequestDto dto);
    Task Checkout(CheckoutMessageDto checkoutMessageDto);

}
public class CheckoutService : ICheckoutService
{
    private readonly AppDbContext _dbContext;
    private readonly KafkaProducer _kafkaProducer;
    private readonly KafkaConfig _kafkaConfig;
    private readonly IMyPayNetApiClient _myPayNetApiClient;
    private readonly IMembershipApiClient _memberApiClient;
    public CheckoutService(AppDbContext dbContext,
                           KafkaProducer kafkaProducer,
                           IOptions<KafkaConfig> kafkaConfig,
                           IMyPayNetApiClient myPayNetApiClient,
                           IMembershipApiClient memberApiClient)
    {
        _dbContext = dbContext;
        _kafkaProducer = kafkaProducer;
        _kafkaConfig = kafkaConfig.Value;
        _myPayNetApiClient = myPayNetApiClient;
        _memberApiClient = memberApiClient;
    }
    public async Task Start(CheckoutRequestDto dto)
    {
        var kafkaMessage= new CheckoutMessageDto
        {
            OrderId = dto.OrderId,
            Email = dto.Email,
            Phone = dto.Phone,
            SelectedCardId = dto.SelectedCardId,
            SelectedAddressId = dto.SelectedAddressId
        };
        await _kafkaProducer.ProduceAsync<CheckoutMessageDto>(kafkaMessage, _kafkaConfig.CheckoutTopic);
    }

    public async Task Checkout(CheckoutMessageDto checkoutMessageDto)
    {
        var order = await _dbContext.Orders.FindAsync(checkoutMessageDto.OrderId);

        if (order == null)
            throw new Exception("Sipariş bulunamadı");


        var card = await RefitWrapper.ExecuteAsync(() => _memberApiClient.GetCard(order.MemberId, checkoutMessageDto.SelectedAddressId));



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
            var paymentError = new Infastructure.Entites.Payment
            {
                OrderId = order.Id,
                IsSuccess = false,
                ErrorMessage = response.Error.Message,
                CreatedAt = DateTime.UtcNow
            };
            await _dbContext.Payments.AddAsync(paymentError);
            await _dbContext.SaveChangesAsync();
            order.PaymentId = paymentError.Id;
            await _dbContext.SaveChangesAsync();

            return;
        }

        var payment = new Infastructure.Entites.Payment
        {
            OrderId = order.Id,
            IsSuccess = true,
            ErrorMessage = string.Empty,
            CreatedAt = DateTime.UtcNow

        };

        await _dbContext.Payments.AddAsync(payment);
        await _dbContext.SaveChangesAsync();

        order.PaymentId = payment.Id;
        await _dbContext.SaveChangesAsync();
        return;

    }

}
