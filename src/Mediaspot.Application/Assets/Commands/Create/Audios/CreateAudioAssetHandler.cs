using Mediaspot.Application.Common;
using Mediaspot.Domain.Assets.AudioAssets;
using Mediaspot.Domain.Assets.ValueObjects;
using MediatR;

namespace Mediaspot.Application.Assets.Commands.Create.Audios;


public sealed class CreateAudioAssetHandler(
    IAssetRepository repo,
    IUnitOfWork uow)
    : IRequestHandler<CreateAudioAssetCommand, Guid>
{
    public async Task<Guid> Handle(
        CreateAudioAssetCommand request,
        CancellationToken ct)
    {
        var existing =
            await repo.GetByExternalIdAsync(
                request.ExternalId,
                ct);

        if (existing is not null)
        {
            throw new InvalidOperationException(
                $"Asset with ExternalId '{request.ExternalId}' already exists.");
        }

        var asset = new AudioAsset(
            request.ExternalId,
            new Metadata(
                request.Title,
                request.Description,
                request.Language),
            request.Duration,
            request.Bitrate,
            request.SampleRate,
            request.Channels);

        await repo.AddAsync(asset, ct);

        await uow.SaveChangesAsync(ct);

        return asset.Id;
    }
}

