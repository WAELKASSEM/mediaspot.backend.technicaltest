### General Solution Improvements
1- The web api project uses a screaming architecture by resource(entity).
2- The web api project has support for global exception handling and ProblemDetails response
   enabling a standardized experience with error handling. Stack traces are only returned in development mode.
3- The solution operates in .net10 for performance improvements.
4- All entities are using Guidv7 for better performance and keyset pagination.
5- The solution uses a generic repository pattern with a base repository for common operations.
6- Support for a real ACID Database has been added with a Postgres database provider instead of an in-memory database.
7- Observability has been added with Microsoft Aspire
8- To reduce friction with production the solution runs locally in an Aspire orchestration. Allowing to containerize
the database, the web api , the queue and the worker.
9- Support for integration tests has been added using Aspire.
10- A queue has been added to the solution RabbitMQ.


### Task 1
Simple Title CRUD.
1- Create a title with some nullable properties.
2- List titles with keyset pagination (Guidv7) and pagesize for better performance.
3- Update a title with some nullable properties. if a property is null in the dto it won't be updated.
   The objective behind this action is to avoid erasing meaningful information.
4- Get a title by id.
5- the title name search is case insensitive using db collation instead of performing to lower and to upper.
6- Improvements for a production scenario: using dtos in responses instead of guids. I prefer contracts so it doesn't breaking consumers but this project is a demo only.

### Task 2 + part of 3 ( transcode job state management)
1- Preset property is now a value object because it has a specific meaning and behavior in more production scenarios.
2- Numerous additional properties have been added the most important ones are : 
 -> Failure Reason (nullable) because i believe that the failure is a domain information and not only an event data.
 -> Verison for concurrency issues as modifying the state of a job is a business logic that cannot be enforced by database (this way we handle concurrency)
3-Changing states of a job is done by entity methods only.
4- 5 transcode job use cases + endpoints have been added. Create(StatePending)/Get/Start/Fail/Complete.
5- Modifying the state of a job raises a domain event. transcode job creation pushes a message (jobId => improvable to multiple fields) to a rabbitMQ through an event handler.
 (We can also create a DLQ for messages through the handler of failedJob domain event)
6- The message is then consumed by a worker. I'll talk about this guy later...

### Task 3 + Worker
1- An endpoint for creating a video asset has been created.
2- Another endpoint for creating an audio asset has been created.
3- Improvement=> the get asset endpoint should return a type discriminator or even better : 
   get /assets/{id} should return a base asset dto with a type discriminator.
   get /assets/video/{id} should return a video asset dto.
   get /assets/audio/{id} should return an audio asset dto.
   The Actual Endpoint get endpoint returns different dtos with a type discriminator allowing polymorphic deserializiation.
4- Improvement=> all properties of assets should be value objects, i have only written logic for resolution, but logic also applies for bitRate for example.
5- About the worker : 
In a real production environment the worker is a container running on its own, not in the same project as api
for better isolation,and custom vm specification.
That's why i took some time to completely isolate it.
now the worker only calls the web api to update a transcode job status
and it's a rabbit mq subscriber to get the latest jobs.
6- If calling the web api is not convenient, it's better to export the Application as a separate nuget package so it can be consumed by the worker.

### How to run the project.
1- update your visual studio software to the latest version
2- have docker running
3- run the AppHost. first launch will take some time in order to install some images (pgsql, rabbitMQ)
   




### Notes
1- For this project ( A simple proof of concept) I decided to focus on integration tests instead of unit tests,
as they allow me to test more functionlities and integration between components with fewer tests.