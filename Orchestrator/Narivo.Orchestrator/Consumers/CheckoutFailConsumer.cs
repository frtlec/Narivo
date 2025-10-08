using Microsoft.Extensions.Options;
using Narivo.Orchestrator.EventStore;
using Narivo.Orchestrator.EventStore.Enums;
using Narivo.Orchestrator.EventStore.Events;
using Narivo.Orchestrator.Sync;
using Narivo.Orchestrator.SyncHttpClient.Dtos;
using Narivo.Shared.Dtos.KafkaMessages;
using Narivo.Shared.Kafka;

namespace Narivo.Orchestrator.Consumers;

public class CheckoutFailConsumer : ConsumerBase
{
    private readonly EventStoreRepository _eventStore;
    private readonly KafkaProducer _kafkaProducer;
    private readonly ICheckoutApiClient _checkoutApiClient;
    public CheckoutFailConsumer(IOptions<KafkaConfig> options, EventStoreRepository eventStoreRepository, KafkaProducer kafkaProducer, ICheckoutApiClient checkoutApiClient) : base(options)
    {
        _eventStore = eventStoreRepository;
        _kafkaProducer = kafkaProducer;
        _checkoutApiClient = checkoutApiClient;
    }

    public override async Task Listiner(CancellationToken cancellationToken)
    {
        await Consume<CheckoutFailMessageDto>(_kafkaConfig.CheckoutFailTopic, async message =>
        {

            await _eventStore.AppendEventAsync(
                            message.OrderId,
                            EventType.CheckoutFailed,
                            new CheckoutFailedEvent
                           (
                                message.OrderId,
                                message.Reason,
                                message.CorrelationId
                            )
                        );


            var initialEvent = await _eventStore.GetEventByTypeAndCorrelationId<CheckoutInitialEvent>(message.OrderId, EventType.CheckoutInitial, message.CorrelationId);

            var failRetryCount = await _eventStore.GetEventTypeCountAsync(message.OrderId, EventType.CheckoutFailed);

            if (initialEvent == null)
                return; // Log: initial event bulunamadı

            if (failRetryCount > initialEvent.MaxRetryCount)
            {
                await _checkoutApiClient.SendResultToClient(new CheckoutSendResultToClient
                {
                    CorrelationId = message.CorrelationId,
                    IsSuccess = false,
                    Message = message.Reason
                });
                return;
            }

            await _kafkaProducer.ProduceAsync<CheckoutInitialMessageDto>(_kafkaConfig.CheckoutInitialTopic, new CheckoutInitialMessageDto
            {
                OrderId = message.OrderId,
                Email = initialEvent.Email,
                Phone = initialEvent.Phone,
                SelectedCardId = initialEvent.SelectedCardId,
                SelectedAddressId = initialEvent.SelectedAddressId,
                MaxRetryCount = initialEvent.MaxRetryCount,
                CorrelationId = message.CorrelationId
            });

        }, cancellationToken);
    }
}