using Mediaspot.Domain.Transcoding;
using MediatR;

namespace Mediaspot.Application.Transcoding.Queries.GetJobById;

public sealed record GetTranscodeJobByIdQuery(Guid JobId) : IRequest<TranscodeJob>;
