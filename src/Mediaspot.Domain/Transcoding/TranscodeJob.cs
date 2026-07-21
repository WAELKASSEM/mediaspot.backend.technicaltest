using Mediaspot.Domain.Common;
using Mediaspot.Domain.Transcoding.Events;
using Mediaspot.Domain.Transcoding.Exceptions;
using Mediaspot.Domain.Transcoding.ValueObjects;

namespace Mediaspot.Domain.Transcoding;

public enum TranscodeStatus { Pending, Running, Succeeded, Failed }

public sealed class TranscodeJob : AggregateRoot
{
    public Guid AssetId { get; private set; }
    public Guid MediaFileId { get; private set; }
    public Preset Preset { get; private set; }
    public TranscodeStatus Status { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private TranscodeJob()
    {
        AssetId = Guid.Empty;
        MediaFileId = Guid.Empty;
        Preset = new(string.Empty);
        Status = TranscodeStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public TranscodeJob(Guid assetId, Guid mediaFileId, Preset preset)
    {
        AssetId = assetId; MediaFileId = mediaFileId; Preset = preset; Status = TranscodeStatus.Pending;
        CreatedAt = DateTime.UtcNow;
        Raise(new TranscodeJobCreated(Id, AssetId, MediaFileId, Preset.Value));
    }
    public void MarkRunning()
    {
        if (Status != TranscodeStatus.Pending)
            throw InvalidTranscodeStatusException.OnlyPendingJobsCanBeStarted(Id, Status);

        Status = TranscodeStatus.Running;
        UpdatedAt = DateTime.UtcNow;
        Raise(new TranscodeJobStarted(Id));
    }

    public void MarkSucceeded()
    {
        if (Status != TranscodeStatus.Running)
            throw InvalidTranscodeStatusException.OnlyRunningJobsCanBeMarkedAsSucceeded(Id, Status);

        Status = TranscodeStatus.Succeeded;
        UpdatedAt = DateTime.UtcNow;
        Raise(new TranscodeJobCompleted(Id));
    }

    public void MarkFailed(string reason)
    {
        if (Status != TranscodeStatus.Running)
            throw InvalidTranscodeStatusException.OnlyRunningJobsCanBeMarkedAsFailed(Id, Status);
        Status = TranscodeStatus.Failed;
        UpdatedAt = DateTime.UtcNow;
        Raise(new TranscodeJobFailed(Id,reason));
    }
}