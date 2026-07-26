using Mediaspot.Application.Common;
using Mediaspot.Application.Common.Exceptions;
using Mediaspot.Domain.Transcoding;
using MediatR;

namespace Mediaspot.Application.Transcoding.Queries.GetJobById;

public sealed class GetTranscodeJobByIdHandler(
    ITranscodeJobRepository repo
) : IRequestHandler<GetTranscodeJobByIdQuery, TranscodeJob>
{
    public async Task<TranscodeJob> Handle(GetTranscodeJobByIdQuery request, CancellationToken cancellationToken)
    {
        var job = await repo.GetAsync(request.JobId, cancellationToken)
            ?? throw EntityNotFoundException.ForType<TranscodeJob>(request.JobId);

        return job;
    }
}
