using Mediaspot.Application.Common;
using Mediaspot.Domain.Transcoding.Events;
using MediatR;

namespace Mediaspot.Application.Transcoding.EventHandlers;

public sealed class TranscodeRequestedHandler(ITranscodeQueuePublisher queuePublisher)
    : INotificationHandler<TranscodeJobCreated>,
      INotificationHandler<TranscodeJobStarted>,
      INotificationHandler<TranscodeJobCompleted>,
      INotificationHandler<TranscodeJobFailed>
{
    public async Task Handle(TranscodeJobCreated notification, CancellationToken cancellationToken)
    {
        await queuePublisher.PublishAsync(notification.JobId, cancellationToken);
    }

    public async Task Handle(TranscodeJobCompleted notification, CancellationToken cancellationToken)
    {
    }

    public async Task Handle(TranscodeJobStarted notification, CancellationToken cancellationToken)
    {
    }

    public async Task Handle(TranscodeJobFailed notification, CancellationToken cancellationToken)
    {
        // improvement add to DLQ for example
    }
}
