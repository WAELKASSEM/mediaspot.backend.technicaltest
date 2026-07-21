using MediatR;

namespace Mediaspot.Application.Transcoding.Commands.StartJob;

public sealed record StartTranscodeJobCommand(Guid JobId) : IRequest<Guid>;
