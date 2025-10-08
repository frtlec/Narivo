using Microsoft.Extensions.Options;
using Narivo.Checkout.Core.Business.Services;
using Narivo.Shared.Dtos.KafkaMessages;
using Narivo.Shared.Exceptions;
using Narivo.Shared.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narivo.Checkout.Consumer.Consumers;
public class CheckoutConsumer : ConsumerBase
{
    private readonly ICheckoutService _checkoutService;
    private readonly KafkaProducer _kafkaProducer;
    public CheckoutConsumer(IOptions<KafkaConfig> options, ICheckoutService checkoutService, KafkaProducer kafkaProducer) : base(options)
    {
        _checkoutService = checkoutService;
        _kafkaProducer = kafkaProducer;
    }

    public override async Task Listiner(CancellationToken cancellationToken)
    {
        await Consume<CheckoutStartMessageDto>(_kafkaConfig.CheckoutStartTopic, async message =>
        {
            try
            {
                await _checkoutService.Checkout(message);
            }
            catch (AppException ex)
            {
                //log 
                await _kafkaProducer.ProduceAsync<CheckoutFailMessageDto>(_kafkaConfig.CheckoutFailTopic, new CheckoutFailMessageDto
                {
                    OrderId = message.OrderId,
                    Reason = ex.Message,
                    CorrelationId = message.CorrelationId,
                });
            }
            catch (Exception ex)
            {
                //log 
                var logId = Guid.NewGuid().ToString();  
                await _kafkaProducer.ProduceAsync<CheckoutFailMessageDto>(_kafkaConfig.CheckoutFailTopic, new CheckoutFailMessageDto
                {
                    OrderId = message.OrderId,
                    Reason = $"Beklenmedik bir hata! LogId:{logId}",
                    CorrelationId = message.CorrelationId,
                });
            }

        }, cancellationToken);
    }
}