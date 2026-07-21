using MediatR;

namespace Mediaspot.Application.Transcoding.Commands.CreateJob;

public sealed record CreateTranscodeJobCommand(
    Guid AssetId,
    Guid MediaFileId,
    string PresetValue
) : IRequest<Guid>;
