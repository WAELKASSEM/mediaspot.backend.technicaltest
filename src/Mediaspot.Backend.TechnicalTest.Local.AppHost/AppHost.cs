var builder = DistributedApplication.CreateBuilder(args);
var postgres = builder.AddPostgres("postgres");
var database = postgres.AddDatabase("Database", databaseName: "MediaSpot");

var rabbitmq = builder.AddRabbitMQ("Queuing");


var webapi = builder.AddProject<Projects.Mediaspot_Api>("mediaspot-api")
        .WithReference(database)
        .WaitFor(database)
        .WithReference(rabbitmq)
        .WaitFor(rabbitmq);

var worker = builder.AddProject<Projects.Mediaspot_Worker>("mediaspot-worker")
             .WithReference(rabbitmq)
             .WithReference(webapi)
             .WithReference(database)
             .WaitFor(rabbitmq)
             .WaitFor(webapi)
             .WaitFor(database);


await builder.Build().RunAsync(CancellationToken.None);

