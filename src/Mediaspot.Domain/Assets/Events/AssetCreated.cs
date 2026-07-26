using Mediaspot.Domain.Common;

namespace Mediaspot.Domain.Assets.Events;

public abstract record AssetCreated(Guid AssetId) : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}
