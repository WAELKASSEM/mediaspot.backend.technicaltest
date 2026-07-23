using Mediaspot.Application.Transcoding;
using Mediaspot.Infrastructure.Queuing.Transcoding;
using Microsoft.Extensions.DependencyInjection;

namespace Mediaspot.Infrastructure.Queuing;

public static class DependencyInjection
{
    public static IServiceCollection AddQueueing(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<RabbitMqConnectionProvider>(sp => new(connectionString));
        services.AddScoped<ITranscodeQueuePublisher, RabbitMqTranscodeQueuePublisher>();
        return services;

    }

}
