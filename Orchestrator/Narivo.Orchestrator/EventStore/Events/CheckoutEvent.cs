namespace Narivo.Orchestrator.EventStore.Events;

public record BaseEvent(
  string CorrelationId
);

public record CheckoutInitialEvent(
    int OrderId,
    string Email,
    string Phone,
    int MaxRetryCount,
    int SelectedCardId,
    int SelectedAddressId,
    string CorrelationId
): BaseEvent(CorrelationId);


public record CheckoutStartEvent(
    int OrderId,
    string Email,
    string Phone,
    int SelectedCardId,
    int SelectedAddressId,
    string CorrelationId
) : BaseEvent(CorrelationId);

public record CheckoutSuccessfulEvent(
    int OrderId,
    string CorrelationId
) : BaseEvent(CorrelationId);

public record CheckoutFailedEvent(
    int OrderId,
    string Reason,
    string CorrelationId
) : BaseEvent(CorrelationId);

public record OrderShippedEvent(
    int OrderId,
    string Reason,
    string CorrelationId
) : BaseEvent(CorrelationId);