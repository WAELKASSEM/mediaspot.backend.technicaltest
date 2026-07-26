namespace Mediaspot.Application.Transcoding;

public interface ITranscodeQueuePublisher
{
    Task PublishAsync(Guid jobId, CancellationToken ct = default);
}
