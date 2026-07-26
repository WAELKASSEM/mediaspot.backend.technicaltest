using Mediaspot.Domain.Assets;

namespace Mediaspot.Application.Common;

public interface IAssetRepository : IRepository<Asset>
{
    Task<Asset?> GetByExternalIdAsync(string externalId, CancellationToken ct);
}
