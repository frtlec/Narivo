using Microsoft.Extensions.Options;
using Narivo.Orchestrator.EventStore;
using Narivo.Orchestrator.EventStore.Enums;
using Narivo.Orchestrator.EventStore.Events;
using Narivo.Shared.Dtos.KafkaMessages;
using Narivo.Shared.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narivo.Orchestrator.Consumers;
public class OrderShippedConsumer : ConsumerBase
{
    private readonly EventStoreRepository _eventStore;
    public OrderShippedConsumer(IOptions<KafkaConfig> options, EventStoreRepository eventStore) : base(options)
    {
        _eventStore = eventStore;
    }

    public override async Task Listiner(CancellationToken cancellationToken)
    {
        await Consume<OrderShippedMessageDto>(_kafkaConfig.OrderShippedTopic, async message =>
        {

            try
            {
                await _eventStore.AppendEventAsync(
                                message.OrderId,
                                EventType.Shipped,
                                new OrderShippedEvent
                               (
                                    message.OrderId,
                                    message.TrackingId,
                                    message.CorrelationId
                                )
                            );

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }, cancellationToken);
    }
}