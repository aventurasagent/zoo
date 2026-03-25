using Zoo.Domain.Entities;

namespace Zoo.Domain.Repositories;

public interface IRepository<T> where T : IAggregateRoot
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}

public interface IAnimalRepository : IRepository<Animal>
{
    Task<IEnumerable<Animal>> GetByEnclosureIdAsync(Guid enclosureId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Animal>> GetBySpeciesAsync(string species, CancellationToken cancellationToken = default);
    Task<IEnumerable<Animal>> GetByStatusAsync(AnimalStatus status, CancellationToken cancellationToken = default);
    Task<IEnumerable<Animal>> GetSickAnimalsAsync(CancellationToken cancellationToken = default);
}

public interface IEnclosureRepository : IRepository<Enclosure>
{
    Task<IEnumerable<Enclosure>> GetByTypeAsync(EnclosureType type, CancellationToken cancellationToken = default);
    Task<IEnumerable<Enclosure>> GetAvailableEnclosuresAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Enclosure>> GetFullEnclosuresAsync(CancellationToken cancellationToken = default);
}

public interface IKeeperRepository : IRepository<Keeper>
{
    Task<IEnumerable<Keeper>> GetActiveKeepersAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Keeper>> GetByEnclosureAsync(Guid enclosureId, CancellationToken cancellationToken = default);
    Task<Keeper?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}

public interface IFeedingScheduleRepository : IRepository<FeedingSchedule>
{
    Task<IEnumerable<FeedingSchedule>> GetByAnimalIdAsync(Guid animalId, CancellationToken cancellationToken = default);
    Task<IEnumerable<FeedingSchedule>> GetActiveSchedulesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<FeedingSchedule>> GetSchedulesForTimeAsync(TimeOnly time, CancellationToken cancellationToken = default);
}
