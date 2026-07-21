using Mediaspot.Application.Common;
using Mediaspot.Application.Transcoding.Commands.CompleteJob;
using Mediaspot.Application.Transcoding.Commands.FailJob;
using Mediaspot.Application.Transcoding.Commands.StartJob;
using Mediaspot.Domain.Transcoding;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Mediaspot.Worker;

public sealed class TranscodeWorker(
    ITranscodeJobRepository repo,
    ISender sender,
    ILogger<TranscodeWorker> logger)
{
    private readonly ITranscodeJobRepository repo = repo;
    private readonly ISender sender = sender;
    private readonly ILogger<TranscodeWorker> logger = logger;

    public async Task RunAsync(CancellationToken ct)
    {
        logger.LogInformation("Simple Transcode Worker started.");

        while (!ct.IsCancellationRequested)
        {
            TranscodeJob? job = await repo.GetNextPendingAsync(ct);

            if (job is null)
            {
                await Task.Delay(1000, ct);
                continue;
            }

            try
            {
                logger.LogInformation("Processing job {JobId}", job.Id);

                await sender.Send(new StartTranscodeJobCommand(job.Id), ct);

                await Task.Delay(3000, ct);

                await sender.Send(new CompleteTranscodeJobCommand(job.Id), ct);

                logger.LogInformation("Job {JobId} completed.", job.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing job {JobId}", job.Id);

                await sender.Send(new FailTranscodeJobCommand(job.Id, ex.Message), ct);
            }
        }
    }
}

