using Narivo.Shared.Kafka;

namespace Narivo.Checkout.Core.Infastructure.Persistence;
public class AppConfig
{
    public MyNetApiConfig MyNetApiConfig { get; set; }
    public KafkaConfig KafkaConfig { get; set; }
}
public record MyNetApiConfig
{
    public string BaseUrl { get; set; }
}