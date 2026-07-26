using Mediaspot.Application.Assets.Commands.Create.Audios;
using Mediaspot.Domain.Assets.ValueObjects;

namespace Mediaspot.Api.Assets.CreateAsset.CreateAudioAsset;
public sealed record CreateAudioAssetDto(
    string ExternalId,
    string Title,
    string? Description,
    string? Language,
    int DurationInSeconds,
    int Bitrate,
    int SampleRate,
    int Channels);
public static class CreateAudioAssetDtoExtensions
{
    public static CreateAudioAssetCommand ToCommand(this CreateAudioAssetDto dto)
    {
        return new CreateAudioAssetCommand(
            dto.ExternalId,
            dto.Title,
            dto.Description,
            dto.Language,
            Duration.FromSeconds(dto.DurationInSeconds),
            dto.Bitrate,
            dto.SampleRate,
            dto.Channels);
    }
}
