namespace Mediaspot.Worker.WebApiDtos;

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
