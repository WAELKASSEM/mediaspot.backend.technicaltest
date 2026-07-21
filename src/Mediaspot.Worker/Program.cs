using Mediaspot.Application.Common;
using Mediaspot.Application.Transcoding.Commands.CreateJob;
using Mediaspot.Infrastructure;
using Mediaspot.Infrastructure.Persistence;
using Mediaspot.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddTransient<TranscodeWorker>();
builder.Services.AddInfrastructure("Mediaspot.Backend.TechnicalTest");


var app = builder.Build();

var worker = app.Services.GetRequiredService<TranscodeWorker>();

await worker.RunAsync(CancellationToken.None);

