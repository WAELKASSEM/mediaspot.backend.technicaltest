using Mediaspot.Application.Common;
using Mediaspot.Application.Common.Exceptions;
using Mediaspot.Domain.Transcoding;
using MediatR;

namespace Mediaspot.Application.Transcoding.Commands.CompleteJob;

public sealed class CompleteTranscodeJobHandler(
    ITranscodeJobRepository repo,
    IUnitOfWork unitOfWork
) : IRequestHandler<CompleteTranscodeJobCommand, Guid>
{
    public async Task<Guid> Handle(CompleteTranscodeJobCommand request, CancellationToken cancellationToken)
    {
        var job = await repo.GetAsync(request.JobId, cancellationToken)
            ?? throw EntityNotFoundException.ForType<TranscodeJob>(request.JobId);
        job.MarkSucceeded();
        await repo.UpdateAsync(job, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return job.Id;
    }
}
