using MediatR;

namespace Mediaspot.Application.Transcoding.Commands.CompleteJob;

public sealed record CompleteTranscodeJobCommand(Guid JobId) : IRequest<Guid>;
