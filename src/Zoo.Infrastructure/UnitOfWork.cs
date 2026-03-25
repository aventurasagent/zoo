using System;
using System.Threading;
using System.Threading.Tasks;
using Zoo.Domain.Repositories;

namespace Zoo.Infrastructure;

/// <summary>
/// Simple unit‑of‑work implementation for the in‑memory demo.
/// All operations are no‑ops because the demo uses in‑memory repositories.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    public Task BeginTransactionAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
    public Task CommitTransactionAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
    public Task RollbackTransactionAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
}