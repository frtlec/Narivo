using Microsoft.Extensions.Options;
using Narivo.Checkout.Core.Business.Services;
using Narivo.Shared.Dtos.KafkaMessages;
using Narivo.Shared.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narivo.Checkout.Consumer.Consumers;


public class OrderShippedConsumer : ConsumerBase
{
    private readonly IOrderService _orderService;
    public OrderShippedConsumer(IOptions<KafkaConfig> options, IOrderService orderService) : base(options)
    {
        _orderService = orderService;
    }

    public override async Task Listiner(CancellationToken cancellationToken)
    {
        await Consume<OrderShippedMessageDto>(_kafkaConfig.OrderShippedTopic, async message =>
        {

            try
            {
                await _orderService.SetShipmentTrackingId(message.OrderId, message.TrackingId);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }, cancellationToken);
    }

}