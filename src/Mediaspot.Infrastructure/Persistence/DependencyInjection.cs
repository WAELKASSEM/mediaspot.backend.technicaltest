using Mediaspot.Application.Assets.Commands.Create.Videos;
using Mediaspot.Application.Common;
using Mediaspot.Application.Titles;
using Mediaspot.Infrastructure.Persistence.Assets;
using Mediaspot.Infrastructure.Persistence.Titles;
using Mediaspot.Infrastructure.Persistence.Transcoding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Mediaspot.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<MediaspotDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<IUnitOfWork,UnitOfWork>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAssetRepository, AssetRepository>();
        services.AddScoped<ITranscodeJobRepository, TranscodeJobRepository>();
        services.AddScoped<ITitleRepository, TitleRepository>();

        // MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateVideoAssetCommand).Assembly));

        return services;
    }
}