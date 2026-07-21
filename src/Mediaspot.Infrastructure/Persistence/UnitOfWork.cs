using Mediaspot.Application.Common;
using Mediaspot.Domain.Common;
using MediatR;

namespace Mediaspot.Infrastructure.Persistence;

public sealed class UnitOfWork(MediaspotDbContext db, IPublisher publisher) : IUnitOfWork
{
    private readonly MediaspotDbContext _db = db;
    private readonly IPublisher _publisher = publisher;

    public Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        // Gather all domain events from tracked aggregates
        var domainEvents = _db.ChangeTracker.Entries<AggregateRoot>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        int result = await _db.SaveChangesAsync(ct);

        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent, ct);
        }

        // Clear domain events after publishing
        foreach (var entity in _db.ChangeTracker.Entries<AggregateRoot>())
        {
            entity.Entity.ClearDomainEvents();
        }

        return result;
    }
}