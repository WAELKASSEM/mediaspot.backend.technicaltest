using Mediaspot.Worker.Transcoders;
using Mediaspot.Worker.WebApiDtos;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net.Http.Json;
using System.Text;

namespace Mediaspot.Worker;


public sealed class TranscodeWorker(
    IConnection connection,
    AssetTranscoderFactory transcoderFactory,
    IHttpClientFactory factory,
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
            cancellationToken: ct);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (_, ea) =>
        {
            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
            var jobId = Guid.Parse(message);

            await Transcode(jobId, ct);

            await channel.BasicAckAsync(
                deliveryTag: ea.DeliveryTag,
                multiple: false);
        };

        await channel.BasicConsumeAsync(
            queue: "transcode-jobs",
            autoAck: false,
            consumer: consumer
            , ct);

        Console.WriteLine("Listening for messages...");

        await Task.Delay(Timeout.Infinite, ct);
    }

    private async Task Transcode(Guid jobId, CancellationToken ct)
    {
        var mediaSpotApiClient = factory.CreateClient("ApiClient");
        try
        {
            logger.LogInformation(
                "Processing transcode job {JobId}",
                jobId);

            TranscodeJobDto job = (await mediaSpotApiClient.GetFromJsonAsync<TranscodeJobDto>($"/transcode-jobs/{jobId}",ct))!;


            var startResult = await mediaSpotApiClient.PutAsync($"/transcode-jobs/{jobId}/start", null, cancellationToken: ct);
            startResult.EnsureSuccessStatusCode();

            var asset = await mediaSpotApiClient.GetFromJsonAsync<AssetDto>($"/assets/{job.AssetId}",options: new() { Converters = {new AssetDtoConverter()} , PropertyNameCaseInsensitive = true },ct);


            var transcoder =
                transcoderFactory.Resolve(asset!);

            await transcoder.ExecuteAsync(
                asset!,
                job,
                ct);

            var completeResult = await mediaSpotApiClient.PutAsync($"/transcode-jobs/{job.Id}/complete", null, cancellationToken: ct);
            completeResult.EnsureSuccessStatusCode();

            logger.LogInformation(
                "Transcode job {JobId} completed",
                job.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Error while processing job {JobId}",
                jobId);

            await mediaSpotApiClient.PutAsync($"/transcode-jobs/{jobId}/failed", null, cancellationToken: ct);
        }

    }
}
