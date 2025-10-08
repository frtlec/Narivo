using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Narivo.Shared.Dtos.KafkaMessages;
using System.Text.Json;

namespace Narivo.Shared.Kafka;
public abstract class ConsumerBase : BackgroundService
{
    protected readonly KafkaConfig _kafkaConfig;
    protected string GroupId => _kafkaConfig.GroupId;
    protected string BootstrapServers => _kafkaConfig.BootstrapServers;
    protected ConsumerConfig ConsumerConfig { get; private set; }
    public ConsumerBase(IOptions<KafkaConfig> options)
    {
        _kafkaConfig = options.Value;

        ConsumerConfig = new ConsumerConfig
        {
            BootstrapServers = BootstrapServers,
            GroupId = _kafkaConfig.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

    }
    public abstract Task Listiner(CancellationToken cancellationToken);
    public virtual async Task Consume<T>(string topic, Func<T, Task> action, CancellationToken cancellationToken)
    {
        using var consumer = new ConsumerBuilder<Ignore, string>(ConsumerConfig).Build();
        consumer.Subscribe(topic);
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(cancellationToken);
                    var message = JsonSerializer.Deserialize<T>(consumeResult.Message.Value);

                    await action.Invoke(message);
                }
                catch
                {

                }
            }
        }
        catch (OperationCanceledException)
        {
            consumer.Close();
        }
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Listiner(stoppingToken);
    }
}