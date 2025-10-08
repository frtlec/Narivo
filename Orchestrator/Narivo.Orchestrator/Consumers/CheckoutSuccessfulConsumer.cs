using Microsoft.Extensions.Options;
using Narivo.Orchestrator.EventStore;
using Narivo.Orchestrator.EventStore.Enums;
using Narivo.Orchestrator.EventStore.Events;
using Narivo.Orchestrator.Sync;
using Narivo.Orchestrator.SyncHttpClient.Dtos;
using Narivo.Shared.Dtos.KafkaMessages;
using Narivo.Shared.Kafka;

namespace Narivo.Orchestrator.Consumers;

public class CheckoutSuccessfulConsumer : ConsumerBase
{
    private readonly EventStoreRepository _eventStore;
    private readonly KafkaProducer _kafkaProducer;
    private readonly ICheckoutApiClient _checkoutApiClient;
    public CheckoutSuccessfulConsumer(IOptions<KafkaConfig> options, EventStoreRepository eventStoreRepository, KafkaProducer kafkaProducer, ICheckoutApiClient checkoutApiClient) : base(options)
    {
        _eventStore = eventStoreRepository;
        _kafkaProducer = kafkaProducer;
        _checkoutApiClient = checkoutApiClient;
    }

    public override async Task Listiner(CancellationToken cancellationToken)
    {
        await Consume<CheckoutSuccessfulMessageDto>(_kafkaConfig.CheckoutSuccessfulTopic, async message =>
        {

            try
            {
                await _eventStore.AppendEventAsync(
                        message.OrderId,
                        EventType.CheckoutInitial,
                        new CheckoutSuccessfulEvent
                       (
                            message.OrderId,
                            message.CorrelationId
                        )
                    );

                await _checkoutApiClient.SendResultToClient(new CheckoutSendResultToClient
                {
                    CorrelationId = message.CorrelationId,
                    IsSuccess = true,
                    Message = "Ödeme işlemi başarılı"
                });
            }
            catch (Exception ex)
            {

                throw;
            }

        }, cancellationToken);
    }
}
