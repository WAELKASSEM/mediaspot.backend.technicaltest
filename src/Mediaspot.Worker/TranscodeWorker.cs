using Mediaspot.Application.Common;
using Mediaspot.Application.Transcoding.Commands.CompleteJob;
using Mediaspot.Application.Transcoding.Commands.FailJob;
using Mediaspot.Application.Transcoding.Commands.StartJob;
using Mediaspot.Domain.Transcoding;
using Mediaspot.Infrastructure.Transcoding;
using MediatR;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Mediaspot.Worker;


public sealed class TranscodeWorker(
    IConnection connection,
    ITranscodeJobRepository transcodeJobRepository,
    IAssetRepository assetRepository,
    AssetTranscoderFactory transcoderFactory,
    ISender sender,
    ILogger<TranscodeWorker> logger)
{

    public async Task StartAsync(CancellationToken ct)
    {
        var channel = await connection.CreateChannelAsync(cancellationToken: ct);

        await channel.QueueDeclareAsync(
            queue: "transcode-jobs",
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken:ct);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (_, ea) =>
        {
            var message = Encoding.UTF8.GetString(ea.Body.ToArray());

            Console.WriteLine($"Received: {message}");

            await channel.BasicAckAsync(
                deliveryTag: ea.DeliveryTag,
                multiple: false);
        };

        await channel.BasicConsumeAsync(
            queue: "transcode-jobs",
            autoAck: false,
            consumer: consumer
            ,ct);

        Console.WriteLine("Listening for messages...");

        await Task.Delay(Timeout.Infinite,ct);
    }

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
