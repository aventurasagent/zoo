using Zoo.Domain.Events;

namespace Zoo.Application.Services;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken = default);
}
