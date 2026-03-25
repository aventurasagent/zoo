using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Zoo.Domain.Entities;
using Zoo.Domain.Exceptions;
using Zoo.Domain.Repositories;

namespace Zoo.Infrastructure.Repositories;

public class EnclosureRepository : IEnclosureRepository
{
    private readonly ConcurrentDictionary<Guid, Enclosure> _entities = new();

    public Task<Enclosure?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        Task.FromResult<Enclosure?>(_entities.TryGetValue(id, out var entity) ? entity : null);

    public Task<IEnumerable<Enclosure>> GetAllAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IEnumerable<Enclosure>>(_entities.Values);

    public Task<Enclosure> AddAsync(Enclosure entity, CancellationToken cancellationToken = default)
    {
        if (entity.Id == Guid.Empty)
            entity.Id = Guid.NewGuid();

        _entities[entity.Id] = entity;
        return Task.FromResult(entity);
    }

    public Task UpdateAsync(Enclosure entity, CancellationToken cancellationToken = default)
    {
        if (!_entities.ContainsKey(entity.Id))
            throw new DomainException("Enclosure not found");

        _entities[entity.Id] = entity;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_entities.TryRemove(id, out _));
    }

    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default) =>
        Task.FromResult(_entities.ContainsKey(id));

    public Task<IEnumerable<Enclosure>> GetByTypeAsync(EnclosureType type, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IEnumerable<Enclosure>>(
            _entities.Values.Where(e => e.Type == type));
    }

    public Task<IEnumerable<Enclosure>> GetAvailableEnclosuresAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IEnumerable<Enclosure>>(
            _entities.Values.Where(e => !e.IsFull()));
    }

    public Task<IEnumerable<Enclosure>> GetFullEnclosuresAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IEnumerable<Enclosure>>(
            _entities.Values.Where(e => e.IsFull()));
    }
}