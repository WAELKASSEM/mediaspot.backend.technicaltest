using Mediaspot.Application.Common;
using Mediaspot.Application.Common.Exceptions;
using Mediaspot.Domain.Transcoding;
using MediatR;

namespace Mediaspot.Application.Transcoding.Commands.FailJob;

public sealed class FailTranscodeJobHandler(
    ITranscodeJobRepository repo,
    IUnitOfWork unitOfWork
) : IRequestHandler<FailTranscodeJobCommand, Guid>
{
    public async Task<Guid> Handle(FailTranscodeJobCommand request, CancellationToken cancellationToken)
    {
        var job = await repo.GetAsync(request.JobId, cancellationToken)
            ?? throw EntityNotFoundException.ForType<TranscodeJob>(request.JobId);

        job.MarkFailed(request.FailureReason);
        await repo.UpdateAsync(job, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return job.Id;
    }
}
