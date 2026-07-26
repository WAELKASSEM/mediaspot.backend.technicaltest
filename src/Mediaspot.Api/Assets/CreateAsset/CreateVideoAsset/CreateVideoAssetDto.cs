using Mediaspot.Application.Assets.Commands.Create.Videos;
using Mediaspot.Domain.Assets.ValueObjects;
using Mediaspot.Domain.Assets.VideoAssets.ValueObjects;

namespace Mediaspot.Api.Assets.CreateAsset.CreateVideoAsset;


public sealed record CreateVideoAssetDto(
    string ExternalId,
    string Title,
    string? Description,
    string? Language,
    int DurationInSeconds,
    int Width,
    int Height,
    decimal FrameRate,
    string Codec);

public static class CreateVideoAssetDtoExtensions
{
    public static CreateVideoAssetCommand ToCommand(this CreateVideoAssetDto dto)
    {
        return new CreateVideoAssetCommand(
            dto.ExternalId,
            dto.Title,
            dto.Description,
            dto.Language,
            Duration.FromSeconds(dto.DurationInSeconds),
            Resolution.Create(dto.Width, dto.Height),
            dto.FrameRate,
            dto.Codec);
    }
}

