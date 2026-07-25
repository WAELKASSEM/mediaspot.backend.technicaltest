using Mediaspot.Worker;
using Mediaspot.Worker.Transcoders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

var builder = Host.CreateApplicationBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Database")!;
var rabbitMqConnectionString = builder.Configuration.GetConnectionString("Queuing")!;
var apiBaseUrl = Environment.GetEnvironmentVariable("MEDIASPOT_API_HTTPS")!;

builder.Services.AddHttpClient("ApiClient", c =>
{
    c.BaseAddress = new Uri(apiBaseUrl);
});


//RabbitMQ
var factory = new ConnectionFactory
{
    Uri = new Uri(rabbitMqConnectionString)
};
var channel = await factory.CreateConnectionAsync();
builder.Services.AddSingleton<IConnection>(x => channel);
//
builder.Services.AddTransient<TranscodeWorker>();
builder.Services.AddTransient<AudioAssetTranscoder>();
builder.Services.AddTransient<VideoAssetTranscoder>();
builder.Services.AddTransient<AssetTranscoderFactory>();



var app = builder.Build();

var worker = app.Services.GetRequiredService<TranscodeWorker>();

await worker.StartAsync(CancellationToken.None);

