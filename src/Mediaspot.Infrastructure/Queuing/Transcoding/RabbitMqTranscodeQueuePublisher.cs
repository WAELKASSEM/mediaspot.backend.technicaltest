using Mediaspot.Application.Transcoding;
using RabbitMQ.Client;
using System.Text;

namespace Mediaspot.Infrastructure.Queuing.Transcoding;

internal sealed class RabbitMqTranscodeQueuePublisher(RabbitMqConnectionProvider connectionProvider)
        : ITranscodeQueuePublisher
{
    public async Task PublishAsync(
        Guid jobId,
        CancellationToken ct = default)
    {
        var connection = await connectionProvider.GetConnectionAsync();
        await using var channel = await connection.CreateChannelAsync(cancellationToken: ct);

        var body = Encoding.UTF8.GetBytes(jobId.ToString());


        await channel.BasicPublishAsync(
            exchange: "",
            routingKey: "transcode-jobs",
            body: body,
            cancellationToken: ct);
    }
}
