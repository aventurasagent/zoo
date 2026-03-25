using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Zoo.Domain.Entities;
using Zoo.Domain.Exceptions;
using Zoo.Domain.Repositories;

namespace Zoo.Infrastructure.Repositories;

public class AnimalRepository : IAnimalRepository
{
    private readonly ConcurrentDictionary<Guid, Animal> _entities = new();

    public Task<Animal?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        Task.FromResult<Animal?>(_entities.TryGetValue(id, out var entity) ? entity : null);

    public Task<IEnumerable<Animal>> GetAllAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IEnumerable<Animal>>(_entities.Values);

    public Task<Animal> AddAsync(Animal entity, CancellationToken cancellationToken = default)
    {
        if (entity.Id == Guid.Empty)
            entity.Id = Guid.NewGuid();

        _entities[entity.Id] = entity;
        return Task.FromResult(entity);
    }

    public Task UpdateAsync(Animal entity, CancellationToken cancellationToken = default)
    {
        if (!_entities.ContainsKey(entity.Id))
            throw new AnimalNotFoundException(entity.Id);

        _entities[entity.Id] = entity;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_entities.TryRemove(id, out _));
    }

    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default) =>
        Task.FromResult(_entities.ContainsKey(id));

    public Task<IEnumerable<Animal>> GetByEnclosureIdAsync(Guid enclosureId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IEnumerable<Animal>>(
            _entities.Values.Where(a => a.EnclosureId == enclosureId));
    }

    public Task<IEnumerable<Animal>> GetBySpeciesAsync(string species, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IEnumerable<Animal>>(
            _entities.Values.Where(a => a.GetType().GetProperty("Species")?.GetValue(a)?.ToString() == species));
    }

    public Task<IEnumerable<Animal>> GetByStatusAsync(AnimalStatus status, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IEnumerable<Animal>>(
            _entities.Values.Where(a => a.Status == status));
    }
}