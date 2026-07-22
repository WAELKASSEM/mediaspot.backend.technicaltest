using Mediaspot.Domain.Assets.ValueObjects;
using MediatR;

namespace Mediaspot.Application.Assets.Commands.Create.Audios;


public sealed record CreateAudioAssetCommand(
    string ExternalId,
    string Title,
    string? Description,
    string? Language,
    Duration Duration,
    int Bitrate,
    int SampleRate,
    int Channels) : IRequest<Guid>;

