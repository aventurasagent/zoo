using Zoo.Domain.Repositories;

namespace Zoo.Application.Services;

public interface IUnitOfWork : IDisposable
{
    IAnimalRepository Animals { get; }
    IEnclosureRepository Enclosures { get; }
    IKeeperRepository Keepers { get; }
    IFeedingScheduleRepository FeedingSchedules { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
