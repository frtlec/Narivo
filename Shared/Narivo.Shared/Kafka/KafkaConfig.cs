namespace Narivo.Shared.Kafka;
public class KafkaConfig
{
    public string BootstrapServers { get; set; } = string.Empty;
    public string CheckoutInitialTopic { get; set; } = string.Empty;
    public string CheckoutStartTopic { get; set; } = string.Empty;
    public string CheckoutSuccessfulTopic { get; set; } = string.Empty;
    public string CheckoutFailTopic { get; set; } = string.Empty;
    public string CheckoutOverRetryFailedTopic { get; set; } = string.Empty;
    public string OrderShippedTopic { get; set; } = string.Empty;
    public string GroupId { get; set; } = string.Empty;
}
