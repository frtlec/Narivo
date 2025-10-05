using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Narivo.Shared.Kafka;

public class KafkaConsumerService : BackgroundService
{
    private readonly KafkaConfig _settings;

    public KafkaConsumerService(IOptions<KafkaConfig> options)
    {
        _settings = options.Value;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() =>
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _settings.BootstrapServers,
                GroupId = _settings.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(_settings.CheckoutTopic);

            Console.WriteLine($"Tüketici {_settings.CheckoutTopic} topic'ini dinliyor...");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var cr = consumer.Consume(stoppingToken);

                    Console.WriteLine($"Alınan mesaj: {cr.Message.Value}");

                }
            }
            catch (OperationCanceledException)
            {
                consumer.Close();
                Console.WriteLine("Consumer durduruldu.");
            }
        }, stoppingToken);
    }
}