using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Zoo.Domain.Entities;
using Zoo.Domain.Exceptions;
using Zoo.Domain.Repositories;

namespace Zoo.Infrastructure.Repositories;

public class KeeperRepository : IKeeperRepository
{
    private readonly ConcurrentDictionary<Guid, Keeper> _entities = new();

    public Task<Keeper?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        Task.FromResult< keeper?>(_entities.TryGetValue(id, out var entity) ? entity : null);

    public Task<IEnumerable<Keeper>> GetAllAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IEnumerable<Keeper>>(_entities.Values);

    public Task<Keeper> AddAsync(Keeper entity, CancellationToken cancellationToken = default)
    {
        if (entity.Id == Guid.Empty)
            entity.Id = Guid.NewGuid();

        _entities[entity.Id] = entity;
        return Task.FromResult(entity);
    }

    public Task UpdateAsync(Keeper entity, CancellationToken cancellationToken = default)
    {
        if (!_entities.ContainsKey(entity.Id))
            throw new KeeperNotFoundException(entity.Id);

        _entities[entity.Id] = entity;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_entities.TryRemove(id, out _));
    }

    public Task<IEnumerable<Keeper>> GetActiveKeepersAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IEnumerable<Keeper>>(_entities.Values.Where(k => k.Status == KeeperStatus.Active));

    public Task<IEnumerable<Keeper>> GetByEnclosureAsync(Guid enclosureId, CancellationToken cancellationToken = default) =>
        Task.FromResult<IEnumerable<Keeper>>(_entities.Values.Where(k => 
            k.AssignedEnclosureIds.Contains(enclosureId)));

    public Task<Keeper?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_entities.Values.FirstOrDefault(k => 
            k.Email.Address.Equals(email, StringComparison.OrdinalIgnoreCase)));
    }
}