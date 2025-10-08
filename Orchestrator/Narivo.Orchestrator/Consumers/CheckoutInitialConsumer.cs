using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Narivo.Orchestrator.EventStore;
using Narivo.Orchestrator.EventStore.Enums;
using Narivo.Orchestrator.EventStore.Events;
using Narivo.Orchestrator.Sync;
using Narivo.Orchestrator.SyncHttpClient.Dtos;
using Narivo.Shared.Dtos.KafkaMessages;
using Narivo.Shared.Kafka;
using System.Text.Json;

namespace Narivo.Orchestrator.Consumers;

public class CheckoutInitialConsumer : ConsumerBase
{

    private readonly EventStoreRepository _eventStore;
    private readonly KafkaProducer _kafkaProducer;
    private readonly ICheckoutApiClient _checkoutApiClient;
    public CheckoutInitialConsumer(IOptions<KafkaConfig> options, EventStoreRepository eventStoreRepository, KafkaProducer kafkaProducer, ICheckoutApiClient checkoutApiClient) : base(options)
    {
        _eventStore = eventStoreRepository;
        _kafkaProducer = kafkaProducer;
        _checkoutApiClient = checkoutApiClient;
    }


    public override async Task Listiner(CancellationToken cancellationToken)
    {
        await Consume<CheckoutInitialMessageDto>(_kafkaConfig.CheckoutInitialTopic, async message =>
        {

            try
            {
                await _eventStore.AppendEventAsync(
                                message.OrderId,
                                EventType.CheckoutInitial,
                                new CheckoutInitialEvent
                               (
                                    message.OrderId,
                                    message.Email,
                                    message.Phone,
                                    message.MaxRetryCount,
                                    message.SelectedCardId,
                                    message.SelectedAddressId,
                                    message.CorrelationId
                                )
                            );
                await _kafkaProducer.ProduceAsync<CheckoutStartMessageDto>(_kafkaConfig.CheckoutStartTopic, new CheckoutStartMessageDto
                {
                    OrderId = message.OrderId,
                    Email = message.Email,
                    Phone = message.Phone,
                    SelectedCardId = message.SelectedCardId,
                    SelectedAddressId = message.SelectedAddressId,
                    CorrelationId = message.CorrelationId
                });

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }, cancellationToken);
    }

}
