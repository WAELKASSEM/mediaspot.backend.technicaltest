using Mediaspot.Infrastructure.Persistence;
using Mediaspot.Infrastructure.Transcoding;
using Mediaspot.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddTransient<TranscodeWorker>();
builder.Services.AddTransient<AudioAssetTranscoder>();
builder.Services.AddTransient<VideoAssetTranscoder>();
builder.Services.AddTransient<AssetTranscoderFactory>();
builder.Services.AddInfrastructure("Mediaspot.Backend.TechnicalTest");



var app = builder.Build();

var worker = app.Services.GetRequiredService<TranscodeWorker>();

await worker.RunAsync(CancellationToken.None);

