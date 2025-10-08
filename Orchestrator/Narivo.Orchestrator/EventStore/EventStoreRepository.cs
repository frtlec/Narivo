using EventStore.Client;
using Microsoft.Extensions.Options;
using Narivo.Orchestrator.EventStore.Enums;
using Narivo.Orchestrator.EventStore.Events;
using System.Text.Json;

namespace Narivo.Orchestrator.EventStore;
public class EventStoreRepository
{
    private readonly EventStoreClient _client;
    private readonly EventStoreSettings _settings;

    public EventStoreRepository(IOptions<EventStoreSettings> options)
    {
        _settings = options.Value;
        var settings = EventStoreClientSettings.Create(_settings.ConnectionString);
        _client = new EventStoreClient(settings);
    }

    private string GetStreamName(int orderId) => $"{_settings.StreamPrefix}{orderId}";

    // Event ekleme
    public async Task AppendEventAsync<T>(int orderId, EventType eventType, T @event) where T : BaseEvent
    {
        var streamName = GetStreamName(orderId);

        var eventData = new EventData(
            Uuid.NewUuid(),
            eventType.ToString(),
            JsonSerializer.SerializeToUtf8Bytes(@event)
        );

        await _client.AppendToStreamAsync(streamName, StreamState.Any, new[] { eventData });
    }

    // Tüm stream'i okuma
    public async Task<List<T>> ReadStreamAsync<T>(int orderId) where T : BaseEvent
    {
        var streamName = GetStreamName(orderId);
        var events = new List<T>();

        var result = _client.ReadStreamAsync(Direction.Forwards, streamName, StreamPosition.Start);

        await foreach (var resolvedEvent in result)
        {
            var obj = JsonSerializer.Deserialize<T>(resolvedEvent.Event.Data.Span);
            if (obj != null) events.Add(obj);
        }

        return events;
    }

    // Son event'i almak
    public async Task<T?> GetLatestEventAsync<T>(int orderId) where T : BaseEvent
    {
        var streamName = GetStreamName(orderId);
        var result = _client.ReadStreamAsync(Direction.Backwards, streamName, StreamPosition.End, maxCount: 1);

        await foreach (var resolvedEvent in result)
        {
            return JsonSerializer.Deserialize<T>(resolvedEvent.Event.Data.Span);
        }

        return default;
    }

    // Son event'in türünü ve verisini almak
    public async Task<EventType?> GetLatestEventType(int orderId)
    {
        try
        {
            var streamName = GetStreamName(orderId);
            var result = _client.ReadStreamAsync(Direction.Backwards, streamName, StreamPosition.End, maxCount: 1);

            await foreach (var resolvedEvent in result)
            {
                if (Enum.TryParse<EventType>(resolvedEvent.Event.EventType, out var eventType))
                    return eventType;
            }
        }
        catch (Exception ex)
        {

            return null;
        }

        return null;
    }

    // belirli bir türdeki ve correlationId'ye sahip event'i almak
    public async Task<T?> GetEventByTypeAndCorrelationId<T>(int orderId, EventType eventType, string correlationId) where T : BaseEvent
    {
        var streamName = GetStreamName(orderId);

        var result = _client.ReadStreamAsync(Direction.Forwards, streamName, StreamPosition.Start);

        await foreach (var resolvedEvent in result)
        {
            if (resolvedEvent.Event.EventType == eventType.ToString())
            {
                var obj = JsonSerializer.Deserialize<T>(resolvedEvent.Event.Data.Span);

                if (obj == null)
                    continue;

                // CorrelationId kontrolü
                var correlationProp = typeof(T).GetProperty("CorrelationId");
                if (correlationProp != null)
                {
                    var value = correlationProp.GetValue(obj)?.ToString();
                    if (value == correlationId)
                        return obj;
                }
            }
        }

        return default;
    }
    public async Task<int> GetEventTypeCountAsync(int orderId, EventType eventType)
    {
        var streamName = GetStreamName(orderId);
        var count = 0;

        var result = _client.ReadStreamAsync(Direction.Forwards, streamName, StreamPosition.Start);

        await foreach (var resolvedEvent in result)
        {
            if (resolvedEvent.Event.EventType == eventType.ToString())
                count++;
        }

        return count;
    }
}
