namespace Mediaspot.Domain.Transcoding.Exceptions;

public class InvalidTranscodeStatusException : Exception
{
    private InvalidTranscodeStatusException(string message) : base(message)
    {
        
    }
    public static InvalidTranscodeStatusException OnlyPendingJobsCanBeStarted(Guid transcodeJobId,TranscodeStatus actual)
    {
        return new InvalidTranscodeStatusException($"Id: {transcodeJobId} - Actual status: {actual}. Only pending jobs can be started.");
    }
    public static InvalidTranscodeStatusException OnlyRunningJobsCanBeMarkedAsSucceeded(Guid transcodeJobId,TranscodeStatus actual)
    {
        return new InvalidTranscodeStatusException($"Id: {transcodeJobId} - Actual status: {actual}. Only running jobs can be marked as succeeded.");
    }
    public static InvalidTranscodeStatusException OnlyRunningJobsCanBeMarkedAsFailed(Guid transcodeJobId,TranscodeStatus actual)
    {
        return new InvalidTranscodeStatusException($"Id: {transcodeJobId} - Actual status: {actual}. Only running jobs can be marked as failed.");
    }

}
