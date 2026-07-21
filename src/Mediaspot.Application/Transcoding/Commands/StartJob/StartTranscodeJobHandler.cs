using Mediaspot.Application.Common;
using Mediaspot.Application.Common.Exceptions;
using Mediaspot.Domain.Transcoding;
using MediatR;

namespace Mediaspot.Application.Transcoding.Commands.StartJob;

public sealed class StartTranscodeJobHandler(
    ITranscodeJobRepository repo,
    IUnitOfWork unitOfWork
) : IRequestHandler<StartTranscodeJobCommand,Guid>
{
    public async Task<Guid> Handle(StartTranscodeJobCommand request, CancellationToken cancellationToken)
    {
        var job = await repo.GetAsync(request.JobId, cancellationToken)??
            throw EntityNotFoundException.ForType<TranscodeJob>(request.JobId);
        job.MarkRunning();
        await repo.UpdateAsync(job, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return job.Id;
    }
}
