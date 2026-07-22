using Mediaspot.Domain.Common;

namespace Mediaspot.Domain.Assets.AudioAssets.Events;

public sealed record AudioAssetUpdated(Guid AssetId) : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}
