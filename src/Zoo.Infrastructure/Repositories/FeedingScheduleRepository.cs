using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Zoo.Domain.Entities;
using Zoo.Domain.Exceptions;
using Zoo.Domain.Repositories;

namespace Zoo.Infrastructure.Repositories;

public class FeedingScheduleRepository : IFeedingScheduleRepository
{
    private readonly ConcurrentDictionary<Guid, FeedingSchedule> _entities = new();

    public Task<FeedingSchedule?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        Task.FromResult<FeedingSchedule?>(_entities.TryGetValue(id, out var entity) ? entity : null)

        .ContinueWith(t => t.Result);

    public Task<IEnumerable<FeedingSchedule>> GetAllAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IEnumerable<FeedingSchedule>>(_entities.Values);

    public Task<FeedingSchedule> AddAsync(FeedingSchedule entity, CancellationToken cancellationToken = default)
    {
        if (entity.Id == Guid.Empty)
            entity.Id = Guid.NewGuid();

        _entities[entity.Id] = entity;
        return Task.FromResult(entity);
    }

    public Task UpdateAsync(FeedingSchedule entity, CancellationToken cancellationToken = default)
    {
        if (!_entities.ContainsKey(entity.Id))
            throw new FeedingScheduleNotFoundException(entity.Id);

        _entities[entity.Id] = entity;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_entities.TryRemove(id, out _));
    }

    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default) =>
        Task.FromResult(_entities.ContainsKey(id));

    public Task<IEnumerable<FeedingSchedule>> GetByAnimalIdAsync(Guid animalId, CancellationToken cancellationToken = default) =>
        Task.FromResult<IEnumerable<FeedingSchedule>>(
            _entities.Values.Where(fs => fs.AnimalId == animalId));

    public Task<IEnumerable<FeedingSchedule>> GetActiveSchedulesAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IEnumerable<FeedingSchedule>>(
            _entities.Values.Where(fs => fs.IsActive));

    public Task<IEnumerable<FeedingSchedule>> GetSchedulesForTimeAsync(TimeOnly time, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IEnumerable<FeedingSchedule>>(
            _entities.Values.Where(fs =>
            {
                var feedingTime = new TimeOnly(fs.FeedingTime.Hour, fs.FeedingTime.Minute);
                return feedingTime.Hour >= time.Hour && feedingTime.Minute >= time.Minute; // simplified
            }));
    }
}