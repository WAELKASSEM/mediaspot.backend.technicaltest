using MediatR;

namespace Mediaspot.Application.Transcoding.Commands.FailJob;

public sealed record FailTranscodeJobCommand(Guid JobId, string FailureReason) : IRequest<Guid>;
