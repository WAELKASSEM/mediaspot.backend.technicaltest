using Mediaspot.Application.Common;
using Mediaspot.Domain.Assets;
using Microsoft.EntityFrameworkCore;

namespace Mediaspot.Infrastructure.Persistence.Assets;
public sealed class AssetRepository(MediaspotDbContext db) : IAssetRepository
{
    public Task<Asset?> GetAsync(Guid id, CancellationToken ct)
        => db.Assets.Include("_mediaFiles").FirstOrDefaultAsync(a => a.Id == id, ct);

    public Task<Asset?> GetByExternalIdAsync(string externalId, CancellationToken ct)
        => db.Assets.FirstOrDefaultAsync(a => a.ExternalId == externalId, ct);

    public async Task AddAsync(Asset asset, CancellationToken ct) => await db.Assets.AddAsync(asset, ct);

    public Task UpdateAsync(Asset entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAsync(Asset entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Asset>> ListAsync(PageSize size, Guid? lastSeen = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
