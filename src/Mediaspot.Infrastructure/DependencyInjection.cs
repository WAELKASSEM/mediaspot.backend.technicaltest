using Mediaspot.Application.Assets.Commands.Create;
using Mediaspot.Application.Common;
using Mediaspot.Application.Titles;
using Mediaspot.Infrastructure.Persistence;
using Mediaspot.Infrastructure.Persistence.Assets;
using Mediaspot.Infrastructure.Persistence.Titles;
using Mediaspot.Infrastructure.Persistence.Transcoding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Mediaspot.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string databaseName)
    {
        services.AddDbContext<MediaspotDbContext>(o => o.UseInMemoryDatabase(databaseName));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAssetRepository, AssetRepository>();
        services.AddScoped<ITranscodeJobRepository, TranscodeJobRepository>();
        services.AddScoped<ITitleRepository, TitleRepository>();

        // MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateAssetCommand).Assembly));

        return services;
    }
}