namespace Narivo.Orchestrator.EventStore;

public class EventStoreSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string StreamPrefix { get; set; } = string.Empty;
}