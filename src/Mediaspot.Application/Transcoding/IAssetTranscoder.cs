using Mediaspot.Domain.Assets;
using Mediaspot.Domain.Transcoding;

namespace Mediaspot.Application.Transcoding;


public interface IAssetTranscoder
{
    Task ExecuteAsync(
        Asset asset,
        TranscodeJob job,
        CancellationToken ct);
}

