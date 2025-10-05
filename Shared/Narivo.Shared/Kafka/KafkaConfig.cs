namespace Narivo.Shared.Kafka;
public class KafkaConfig
{
    public string BootstrapServers { get; set; } = string.Empty;
    public string CheckoutTopic { get; set; } = string.Empty;
    public string GroupId { get; set; } = string.Empty;
}
