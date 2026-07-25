1. Migration .net 10 . facile et gratuit.
2. Entity Id Guid v7 . Acceleration de la recherche.
3. Screaming architecture for web api => [EntityName]/[OperationNameEndpoint and dtos]
4. Added Commmon Repository Interface
5. Updated Unit Of Work Interface ( transaction support)
6. Persistence , screaming architecture + separate file entityTypeConfiguration
7. For Listing Titles => Added Keyset Pagination
8. Add Global Excetion MiddleWare + ProblemDetails. Stack Trace for dev environments only


### Global Changelog
1. Migrate all projects to .net 10

### WebApi 
1. Add Global Exception Middleware. Exceptions are returned as ProblemDetails standard. The stack trace is only returned for development envs.
2. Added Screaming architecture per resource (Entity). Each folder consisting of Dtos, and endpoint mappers.
Improvement Proposal : Create Strongly Typed dtos for responses too. instead of returning guids. I let some slip away because this project is a demo only. But, my preference is to always return contracts.
3. Added Telemetery using the project AppHost.ServiceDefaults.

### Infrastructure
1. Usage of pgsql instead of in memory. Always use a real database for integration tests.
2. Usage of RabbitMQ as a queuing system.

### Task 1
Relatively simple implementation.
Highlighted features : 
-> Case insensitive search by title name using db collation instead of performing to lower and to upper 
-> search titles (List titles) using keyset pagination (Guidv7) for better performance 
Possible improvement : Add and use domain events

### Task 2 
-> adding