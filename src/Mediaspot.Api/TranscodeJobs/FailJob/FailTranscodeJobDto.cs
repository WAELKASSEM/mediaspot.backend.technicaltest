using Mediaspot.Application.Transcoding.Commands.FailJob;

namespace Mediaspot.Api.TranscodeJobs.FailJob;

public record FailTranscodeJobDto(string FailureReason);

public static class FailTranscodeJobDtoExtensions
{
    public static FailTranscodeJobCommand ToCommand(this FailTranscodeJobDto dto, Guid id)
    {
        return new FailTranscodeJobCommand(
            JobId: id,
            FailureReason: dto.FailureReason);
    }
}
