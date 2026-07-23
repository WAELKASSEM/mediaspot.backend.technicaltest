using Mediaspot.Application.Common;
using Mediaspot.Domain.Transcoding;
using Microsoft.EntityFrameworkCore;

namespace Mediaspot.Infrastructure.Persistence.Transcoding;

public sealed class TranscodeJobRepository(MediaspotDbContext db) : ITranscodeJobRepository
{
    public async Task AddAsync(TranscodeJob job, CancellationToken ct) => await db.TranscodeJobs.AddAsync(job, ct);

    public Task<bool> HasActiveJobsAsync(Guid assetId, CancellationToken ct)
        => db.TranscodeJobs.AnyAsync(j => j.AssetId == assetId && (j.Status == TranscodeStatus.Pending || j.Status == TranscodeStatus.Running), ct);

    public async Task<TranscodeJob?> GetAsync(Guid jobId, CancellationToken ct)
        => await db.TranscodeJobs.AsNoTracking().FirstOrDefaultAsync(j => j.Id == jobId, ct);

    public Task UpdateAsync(TranscodeJob job, CancellationToken ct)
    {
        var entry = db.TranscodeJobs.Attach(job);
        entry.State = EntityState.Modified;
        return Task.CompletedTask;
    }

    public Task RemoveAsync(TranscodeJob entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TranscodeJob>> ListAsync(PageSize size, Guid? lastSeen = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<TranscodeJob?> GetNextPendingAsync(CancellationToken ct)
    {
        return db.TranscodeJobs.FirstOrDefaultAsync(j => j.Status == TranscodeStatus.Pending, ct);
    }
}
