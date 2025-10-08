
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Narivo.Shared.Kafka
{
    public class KafkaProducer
    {
        private readonly KafkaConfig _settings;

        public KafkaProducer(IOptions<KafkaConfig> options)
        {
            _settings = options.Value;
        }

        public async Task ProduceAsync<T>(string topic,T message) where T : class
        {
            var json = JsonSerializer.Serialize(message);

            var config = new ProducerConfig
            {
                BootstrapServers = _settings.BootstrapServers,
                Acks = Acks.None, // cevap beklemeden yolla
                MessageTimeoutMs = 5000
            };

            using var producer = new ProducerBuilder<Null, string>(config).Build();
            var result = await producer.ProduceAsync(topic, new Message<Null, string> { Value = json });
            Console.WriteLine($"Mesaj gönderildi: {result.TopicPartitionOffset}");
        }
    }
}
