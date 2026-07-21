using Mediaspot.Application.Common;
using Mediaspot.Domain.Assets.Events;
using Mediaspot.Domain.Transcoding;
using MediatR;

namespace Mediaspot.Application.Events;

public sealed class TranscodeRequestedHandler(ITranscodeJobRepository repo, IUnitOfWork uow)
    : INotificationHandler<TranscodeRequested>
{
    public async Task Handle(TranscodeRequested @event, CancellationToken ct)
    {
        var job = new TranscodeJob(@event.AssetId, @event.MediaFileId, new(@event.TargetPreset));
        await repo.AddAsync(job, ct);
        await uow.SaveChangesAsync(ct);
    }
}
