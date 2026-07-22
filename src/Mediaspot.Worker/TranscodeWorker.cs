using Mediaspot.Application.Common;
using Mediaspot.Application.Transcoding.Commands.CompleteJob;
using Mediaspot.Application.Transcoding.Commands.FailJob;
using Mediaspot.Application.Transcoding.Commands.StartJob;
using Mediaspot.Domain.Transcoding;
using Mediaspot.Infrastructure.Transcoding;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Mediaspot.Worker;


public sealed class TranscodeWorker(
    ITranscodeJobRepository transcodeJobRepository,
    IAssetRepository assetRepository,
    AssetTranscoderFactory transcoderFactory,
    ISender sender,
    ILogger<TranscodeWorker> logger)
{
    public async Task RunAsync(CancellationToken ct)
    {
        logger.LogInformation("Transcode worker started.");

        while (!ct.IsCancellationRequested)
        {
            var job = await transcodeJobRepository
                .GetNextPendingAsync(ct);

            if (job is null)
            {
                await Task.Delay(1000, ct);
                continue;
            }

            try
            {
                logger.LogInformation(
                    "Processing transcode job {JobId}",
                    job.Id);

                await sender.Send(
                    new StartTranscodeJobCommand(job.Id),
                    ct);

                var asset =
                    await assetRepository.GetAsync(
                        job.AssetId,
                        ct);

                if (asset is null)
                    throw new InvalidOperationException(
                        $"Asset '{job.AssetId}' not found.");

                var transcoder =
                    transcoderFactory.Resolve(asset);

                await transcoder.ExecuteAsync(
                    asset,
                    job,
                    ct);

                await sender.Send(
                    new CompleteTranscodeJobCommand(job.Id),
                    ct);

                logger.LogInformation(
                    "Transcode job {JobId} completed",
                    job.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Error while processing job {JobId}",
                    job.Id);

                await sender.Send(
                    new FailTranscodeJobCommand(
                        job.Id,
                        ex.Message),
                    ct);
            }
        }
    }
}


