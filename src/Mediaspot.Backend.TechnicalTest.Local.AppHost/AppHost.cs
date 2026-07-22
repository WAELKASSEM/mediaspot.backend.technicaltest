var builder = DistributedApplication.CreateBuilder(args);
var postgres = builder.AddPostgres("postgres");
var database = postgres.AddDatabase("Database", databaseName: "MediaSpot");


_ = builder.AddProject<Projects.Mediaspot_Api>("mediaspot-api")
        .WithReference(database)
        .WaitFor(database);


await builder.Build().RunAsync(CancellationToken.None);







await builder.Build().RunAsync();