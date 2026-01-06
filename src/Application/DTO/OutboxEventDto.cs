namespace Application.DTO;

public sealed class OutboxEventDto
{
    public long EventId { get; init; }

    public string EventType { get; init; } = string.Empty;

    public string Payload { get; init; } = string.Empty;

    public DateTime OccurredAt { get; init; }
}