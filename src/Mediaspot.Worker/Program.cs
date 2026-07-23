using Mediaspot.Infrastructure.Persistence;
using Mediaspot.Infrastructure.Transcoding;
using Mediaspot.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

var builder = Host.CreateApplicationBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Database")!;
var rabbitMqConnectionString = builder.Configuration.GetConnectionString("Queuing")!;
var apiBaseUrl = builder.Configuration["Api__BaseUrl"]!;

builder.Services.AddHttpClient<MediaSpotApiClient>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
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
builder.Services.AddPersistence(connectionString);



var app = builder.Build();

var worker = app.Services.GetRequiredService<TranscodeWorker>();

await worker.StartAsync(CancellationToken.None);

