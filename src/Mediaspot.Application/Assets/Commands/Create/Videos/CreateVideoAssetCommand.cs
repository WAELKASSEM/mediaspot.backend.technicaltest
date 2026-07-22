using Mediaspot.Domain.Assets.ValueObjects;
using Mediaspot.Domain.Assets.VideoAssets.ValueObjects;
using MediatR;

namespace Mediaspot.Application.Assets.Commands.Create.Videos;

public sealed record CreateVideoAssetCommand(
    string ExternalId,
    string Title,
    string? Description,
    string? Language,
    Duration Duration,
    Resolution Resolution,
    decimal FrameRate,
    string Codec) : IRequest<Guid>;


