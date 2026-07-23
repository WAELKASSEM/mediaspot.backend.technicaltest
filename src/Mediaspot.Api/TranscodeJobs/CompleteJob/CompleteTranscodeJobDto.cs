using Mediaspot.Application.Transcoding.Commands.CompleteJob;

namespace Mediaspot.Api.TranscodeJobs.CompleteJob;

public record CompleteTranscodeJobDto(Guid JobId);

public static class CompleteTranscodeJobDtoExtensions
{
    public static CompleteTranscodeJobCommand ToCommand(this CompleteTranscodeJobDto dto)
    {
        return new CompleteTranscodeJobCommand(JobId: dto.JobId);
    }
}
