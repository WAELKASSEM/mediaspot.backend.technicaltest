using Mediaspot.Domain.Common;

namespace Mediaspot.Application.Common;

public interface IRepository<TEntity>
    where TEntity : Entity
{
    Task<TEntity?> GetAsync(Guid id, CancellationToken cancellationToken = default);

    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> ListAsync(PageSize size,Guid? lastSeen = null, CancellationToken cancellationToken = default);
}
