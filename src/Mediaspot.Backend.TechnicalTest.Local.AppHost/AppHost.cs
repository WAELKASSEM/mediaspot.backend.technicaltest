var builder = DistributedApplication.CreateBuilder(args);
var postgres = builder.AddPostgres("postgres");
var database = postgres.AddDatabase("Database", databaseName: "MediaSpot");

var rabbitmq = builder.AddRabbitMQ("Queuing");


var webapi = builder.AddProject<Projects.Mediaspot_Api>("mediaspot-api")
        .WithReference(database)
        .WaitFor(database)
        .WithReference(rabbitmq)
        .WaitFor(rabbitmq);


var enableWorker = Environment.GetEnvironmentVariable("disableWorker") == null;

if (enableWorker)
{
    var worker = builder.AddProject<Projects.Mediaspot_Worker>("mediaspot-worker")
                 .WithReference(rabbitmq)
                 .WithReference(webapi)
                 .WaitFor(rabbitmq)
                 .WaitFor(webapi);
}

await builder.Build().RunAsync(CancellationToken.None);

