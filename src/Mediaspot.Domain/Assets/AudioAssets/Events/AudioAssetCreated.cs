using Mediaspot.Domain.Assets.Events;

namespace Mediaspot.Domain.Assets.AudioAssets.Events;

public sealed record AudioAssetCreated(Guid AssetId) : AssetCreated(AssetId);

