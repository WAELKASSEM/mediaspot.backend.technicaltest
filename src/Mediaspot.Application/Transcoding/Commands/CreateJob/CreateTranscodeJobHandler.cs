using Mediaspot.Application.Common;
using Mediaspot.Domain.Transcoding;
using Mediaspot.Domain.Transcoding.ValueObjects;
using MediatR;

namespace Mediaspot.Application.Transcoding.Commands.CreateJob;

public sealed class CreateTranscodeJobHandler(
    ITranscodeJobRepository repo,
    IUnitOfWork unitOfWork
) : IRequestHandler<CreateTranscodeJobCommand, Guid>
{
    public async Task<Guid> Handle(CreateTranscodeJobCommand request, CancellationToken cancellationToken)
    {
        var preset = new Preset(request.PresetValue);
        var job = new TranscodeJob(request.AssetId, request.MediaFileId, preset);

        await repo.AddAsync(job, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return job.Id;
    }
}
