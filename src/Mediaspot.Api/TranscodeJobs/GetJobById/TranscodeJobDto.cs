using Mediaspot.Domain.Transcoding;

namespace Mediaspot.Api.TranscodeJobs.GetJobById;

public record TranscodeJobDto(
    Guid Id,
    Guid AssetId,
    Guid MediaFileId,
    string Preset,
    string Status,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    string? FailureReason
);

public static class GetTranscodeJobByIdDtoExtensions
{
    public static TranscodeJobDto ToDto(this TranscodeJob job)
    {
        return new TranscodeJobDto(
            Id: job.Id,
            AssetId: job.AssetId,
            MediaFileId: job.MediaFileId,
            Preset: job.Preset.Value,
            Status: job.Status.ToString(),
            CreatedAt: job.CreatedAt,
            UpdatedAt: job.UpdatedAt,
            FailureReason: job.FailureReason
        );
    }
}
