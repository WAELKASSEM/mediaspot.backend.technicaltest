using Mediaspot.Application.Transcoding.Commands.CreateJob;

namespace Mediaspot.Api.TranscodeJobs.CreateJob;

public record CreateTranscodeJobDto(
    Guid AssetId,
    Guid MediaFileId,
    string PresetValue
);

public static class CreateTranscodeJobDtoExtensions
{
    public static CreateTranscodeJobCommand ToCommand(this CreateTranscodeJobDto dto)
    {
        return new CreateTranscodeJobCommand(
            AssetId: dto.AssetId,
            MediaFileId: dto.MediaFileId,
            PresetValue: dto.PresetValue);
    }
}
