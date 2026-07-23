//Defined outside of namespace to be called OnlyOnce per Assembly.
// Why once per assembly ? Because we want to start the application once and share the HttpClient across all tests, which is more efficient and avoids issues with multiple instances of the application running simultaneously. + Performance
#pragma warning disable
using Aspire.Hosting;
using Aspire.Hosting.Testing;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Projects;

[SetUpFixture]
public class AspireSingletonFeature
{
    public static HttpClient ApiClient { get; private set; } = default!;
    private static DistributedApplication app;

    [OneTimeSetUp]
    public async Task GlobalSetup()
    {
        IDistributedApplicationTestingBuilder appHost = await DistributedApplicationTestingBuilder.CreateAsync<Mediaspot_Backend_TechnicalTest_Local_AppHost>();
        app = await appHost.BuildAsync();
        await app.StartAsync();
        ApiClient = app.CreateHttpClient("mediaspot-api");
        

    }

    [OneTimeTearDown]
    public async Task GlobalTeardown()
    {
        ApiClient.Dispose();
        await app.DisposeAsync();
    }
}
