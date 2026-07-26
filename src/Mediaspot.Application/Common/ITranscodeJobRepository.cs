using Mediaspot.Domain.Transcoding;

namespace Mediaspot.Application.Common;

public interface ITranscodeJobRepository : IRepository<TranscodeJob>
{
    Task<TranscodeJob?> GetNextPendingAsync(CancellationToken ct);
    Task<bool> HasActiveJobsAsync(Guid assetId, CancellationToken ct);
}
