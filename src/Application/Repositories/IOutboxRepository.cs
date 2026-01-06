using Application.DTO;

namespace Application.Repositories;

public interface IOutboxRepository
{
    Task AddAsync(
        OutboxEventDto evt,
        CancellationToken ct);
}