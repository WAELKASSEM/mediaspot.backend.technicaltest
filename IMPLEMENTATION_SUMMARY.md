# Mediaspot Backend Technical Test - Implementation Summary

## Overview
This document summarizes the complete implementation of the Mediaspot Backend Technical Test, including all required tasks, architectural improvements, and technology enhancements.

---

## ✅ Tasks Completed

### **Basic Tasks**
#### ✓ Title Domain Entity
- **Location**: `src/Mediaspot.Domain/Titles/Title.cs`
- **Properties**:
  - `Id` (inherited from AggregateRoot)
  - `Name` (TitleName value object - immutable)
  - `Description` (nullable, TitleDescription value object)
  - `ReleaseDate` (nullable, ReleaseDate value object)
  - `Type` (TitleType enum: Movie, Series, Documentary, etc.)

- **Business Rules**:
  - Title name uniqueness enforced at repository level (database unique constraint)
  - Titles are immutable aggregate roots following DDD principles
  - Factory pattern for safe object creation (`Title.Create()`)
  - Methods for updating individual properties: `Rename()`, `ChangeDescription()`, `ChangeReleaseDate()`, `ChangeType()`

- **API Endpoints**:
  - `POST /titles` - Create a new title
  - `PUT /titles/{id}` - Update an existing title
  - `GET /titles/{id}` - Retrieve a specific title
  - `GET /titles` - List all titles with pagination

- **Implementation Details**:
  - All properties use value objects for type safety
  - Aggregate root pattern with event publishing (placeholder for domain events)
  - DTOs for API contracts (`CreateTitleDto`, `UpdateTitleDto`, `TitleDto`)
  - Repository pattern: `ITitleRepository` with `TitleRepository` implementation

---

### **Middle Tasks**
#### ✓ TranscodeJob Domain Model (Event-Driven)
- **Location**: `src/Mediaspot.Domain/Transcoding/TranscodeJob.cs`
- **Properties**:
  - `Id` (aggregate root identifier)
  - `AssetId` (reference to associated asset)
  - `MediaFileId` (reference to specific media file being transcoded)
  - `Preset` (TranscodePreset value object: e.g., 'HD_720p', 'Mobile_Low')
  - `Status` (enum: Pending, Running, Succeeded, Failed)
  - `CreatedAt`, `UpdatedAt` (datetime tracking)
  - `FailureReason` (nullable, stores error details)
  - `Version` (for optimistic concurrency)

- **Domain Events**:
  - `TranscodeJobCreated` - Published when job is created
  - `TranscodeJobStarted` - Published when job transitions to Running
  - `TranscodeJobCompleted` - Published when job succeeds
  - `TranscodeJobFailed` - Published when job fails (includes failure reason)
  - **Location**: `src/Mediaspot.Domain/Transcoding/Events/`

- **Business Rules** (Status Transitions):
  - Only Pending jobs can transition to Running (enforced via `MarkRunning()`)
  - Only Running jobs can succeed (enforced via `MarkSucceeded()`)
  - Only Running jobs can fail (enforced via `MarkFailed()`)
  - Custom exception: `InvalidTranscodeStatusException` for invalid transitions
  - **Location**: `src/Mediaspot.Domain/Transcoding/Exceptions/`

- **Use Cases** (CQRS Pattern):
  - `CreateTranscodeJobCommand` / `CreateTranscodeJobHandler` - Initialize new job
  - `StartTranscodeJobCommand` / `StartTranscodeJobHandler` - Mark job as running
  - `CompleteTranscodeJobCommand` / `CompleteTranscodeJobHandler` - Mark job as succeeded
  - `FailTranscodeJobCommand` / `FailTranscodeJobHandler` - Mark job as failed
  - `GetTranscodeJobByIdQuery` / `GetTranscodeJobByIdHandler` - Retrieve job details
  - **Location**: `src/Mediaspot.Application/Transcoding/Commands/` & `Queries/`

- **Event Handling**:
  - `TranscodeRequestedHandler` - Listens to `TranscodeRequested` domain event from Asset
  - Automatically creates transcode jobs when transcoding is requested
  - **Location**: `src/Mediaspot.Application/Transcoding/EventHandlers/`

- **API Endpoints**:
  - `POST /transcode-jobs` - Create a new transcoding job
  - `PATCH /transcode-jobs/{id}/start` - Start job processing
  - `PATCH /transcode-jobs/{id}/complete` - Mark job as completed
  - `PATCH /transcode-jobs/{id}/fail` - Mark job as failed
  - `GET /transcode-jobs/{id}` - Retrieve job status

---

#### ✓ Worker Application
- **Location**: `src/Mediaspot.Worker/`
- **Components**:
  - `TranscodeWorker` - Orchestrates job processing
  - `MediaSpotApiClient` - HTTP client for communicating with main API
  - **Files**: `Program.cs`, `TranscodeWorker.cs`, `MediaSpotApiClient.cs`

- **Features**:
  - Receives transcode job messages from RabbitMQ queue
  - Polls for pending transcode jobs from the database
  - Updates job status through API calls
  - Handles graceful shutdown

- **Implementation**:
  - Consumer pattern integrated with RabbitMQ
  - Polls database for pending jobs (dummy processing with configurable delay)
  - Sends completion/failure status back to API via HTTP
  - Dependency injection with Azure Service Defaults

- **Processing Flow**:
  1. Worker connects to RabbitMQ and Database
  2. Polls for jobs in Pending status
  3. Transitions job to Running via API
  4. Simulates processing (configurable delay)
  5. Marks job as Succeeded or Failed based on processing result

---

### **Advanced Tasks**
#### ✓ Abstract Asset with Specialized Implementations

**Asset Base Class** - `src/Mediaspot.Domain/Assets/Asset.cs`
- Abstract aggregate root (sealed children only)
- Generic properties:
  - `ExternalId` - External system identifier
  - `Metadata` (Metadata value object) - Title, description, additional metadata
  - `Archived` - Boolean flag for archival status
  - `MediaFiles` - Collection of associated media files

- **Domain Operations**:
  - `RegisterMediaFile(FilePath, Duration)` - Add media file to asset
  - `UpdateMetadata(Metadata)` - Update asset metadata with invariant validation
  - `Archive(Func<Guid, bool> hasActiveJobs)` - Archive asset if no active jobs

- **Domain Events**:
  - `AssetCreated` - Published on asset creation
  - `MediaFileRegistered` - Published when media file added
  - `MetadataUpdated` - Published when metadata changed
  - `AssetArchived` - Published when asset archived
  - `TranscodeRequested` - Published to trigger transcode jobs
  - **Location**: `src/Mediaspot.Domain/Assets/Events/`

---

#### ✓ AudioAsset
- **Location**: `src/Mediaspot.Domain/Assets/AudioAssets/AudioAsset.cs`
- **Audio-Specific Properties**:
  - `Duration` (Duration value object - hours, minutes, seconds)
  - `Bitrate` (int - kbps)
  - `SampleRate` (int - Hz)
  - `Channels` (int - mono, stereo, etc.)

- **Audio-Specific Operations**:
  - `UpdateTechnicalMetadata()` - Update audio properties
  - `AudioAssetCreated` event on instantiation
  - `AudioAssetUpdated` event after technical metadata update

- **Implementation**:
  - Sealed class inheriting from abstract Asset
  - Factory pattern through constructor
  - EF Core configuration in `AudioAssetConfiguration`

---

#### ✓ VideoAsset
- **Location**: `src/Mediaspot.Domain/Assets/VideoAssets/VideoAsset.cs`
- **Video-Specific Properties**:
  - `Duration` (Duration value object)
  - `Resolution` (Resolution value object - WIDTHxHEIGHT, e.g., 1920x1080)
  - `FrameRate` (decimal - fps)
  - `Codec` (string - H.264, H.265, VP9, etc.)

- **Video-Specific Operations**:
  - `UpdateTechnicalMetadata()` - Update video properties
  - `VideoAssetCreated` event on instantiation
  - `VideoAssetUpdated` event after technical metadata update

- **Value Objects**:
  - `Resolution` - Enforces valid resolution format
  - **Location**: `src/Mediaspot.Domain/Assets/VideoAssets/ValueObjects/`

---

#### ✓ Transcoding for Both Asset Types
- **Factory Pattern** - `src/Mediaspot.Infrastructure/Transcoding/AssetTranscoderFactory.cs`
  - `AssetTranscoderFactory` routes to appropriate transcoder based on asset type
  - Type-safe polymorphic transcoding

- **AudioAssetTranscoder** - `src/Mediaspot.Infrastructure/Transcoding/AudioAssetTranscoder.cs`
  - Implements `IAssetTranscoder` interface
  - Audio-specific transcoding logic
  - Handles bitrate, sample rate, channel conversions

- **VideoAssetTranscoder** - `src/Mediaspot.Infrastructure/Transcoding/VideoAssetTranscoder.cs`
  - Implements `IAssetTranscoder` interface
  - Video-specific transcoding logic
  - Handles resolution, frame rate, codec conversions

- **Use Cases**:
  - `CreateAudioAssetCommand` / `CreateAudioAssetHandler`
  - `CreateVideoAssetCommand` / `CreateVideoAssetHandler`
  - **Location**: `src/Mediaspot.Application/Assets/Commands/Create/`

- **API Endpoints**:
  - `POST /assets/audio` - Create audio asset
  - `POST /assets/video` - Create video asset
  - `POST /assets/{id}/media-files` - Register media file
  - `PATCH /assets/{id}/metadata` - Update metadata

---

#### ✓ Transcode Job Management
- **Status Transition Enforcement**:
  - Custom exception: `InvalidTranscodeStatusException`
  - Prevents invalid state transitions at domain level
  - Provides detailed error messages for debugging

- **Archive/De-archive Feature**:
  - Assets can be archived only when no active transcode jobs exist
  - Archival triggers `AssetArchived` event
  - Prevents data loss by checking job status before allowing archive

---

### **Bonus: Archive/De-archive Architecture**
- **Location**: `src/Mediaspot.Domain/Assets/Asset.cs` - `Archive()` method
- **Side Effects Handling**:
  - `Archive()` accepts a predicate function: `Func<Guid, bool> hasActiveJobs`
  - Checks if asset has active jobs before allowing archive
  - Throws `InvalidOperationException` if active jobs exist
  - Raises `AssetArchived` domain event for downstream processing

- **Event Publishing Pattern**:
  - Domain events published via `Raise()` method
  - Handlers subscribe to events asynchronously
  - Example: `TranscodeRequestedHandler` processes archive events

- **Future Enhancement**:
  - Event handlers can trigger API calls to 3rd-party archival services
  - Can implement cloud storage lifecycle policies (S3, Azure Blob)
  - Could implement webhook notifications to external systems

---

### **Bonus 2: Queue System Integration**
- **RabbitMQ Integration**:
  - `RabbitMqConnectionProvider` - `src/Mediaspot.Infrastructure/Queuing/RabbitMqConnectionProvider.cs`
  - Manages RabbitMQ connection lifecycle
  - **Location**: `src/Mediaspot.Infrastructure/Queuing/`

- **Queue Publisher**:
  - `RabbitMqTranscodeQueuePublisher` - Publishes transcode job messages
  - Implemented from `ITranscodeQueuePublisher` interface
  - Integration point for background processing

- **Worker Consumption**:
  - Worker polls RabbitMQ for job messages
  - Processes jobs with configurable delay (simulating actual transcoding)
  - Updates job status via API

---

## 🏗️ Architecture & Design Patterns

### **Domain-Driven Design (DDD)**
- **Aggregate Roots**: `Asset`, `Title`, `TranscodeJob` with clear boundaries
- **Value Objects**: `Duration`, `Metadata`, `Resolution`, `Preset`, `FilePath`, `MediaFileId`, `TitleName`, `TitleDescription`, `ReleaseDate`, `TitleType`
- **Bounded Contexts**: Assets, Titles, Transcoding (separated into different projects)
- **Ubiquitous Language**: Domain events, status states, operations use consistent terminology

### **Event-Driven Architecture**
- **Domain Events**: Published by aggregate roots
- **Event Handlers**: Decoupled consumers using MediatR
- **Event Sourcing Foundation**: Event classes ready for event store implementation

### **CQRS Pattern**
- **Commands**: CreateTitle, UpdateTitle, CreateTranscodeJob, StartTranscodeJob, etc.
- **Queries**: GetTitleById, ListTitles, GetTranscodeJobById
- **Handlers**: MediatR-based requesting/response pattern

### **Repository Pattern**
- **Interfaces**: `ITitleRepository`, `IAssetRepository`, `ITranscodeJobRepository`
- **Implementations**: EF Core-based repositories with DbContext
- **Unit of Work**: `IUnitOfWork` for transaction management

### **Dependency Injection**
- **Extension Methods**: `AddPersistence()`, `AddQueueing()` for service configuration
- **Service Defaults**: Azure Service Defaults integration for Aspire
- **Modular**: Each layer independently configurable

---

## 🗄️ Database: PostgreSQL

### **Migration from Default**
- **Initial**: Project likely used SQL Server or In-Memory
- **Current**: PostgreSQL via EF Core Npgsql provider
- **Connection String**: Managed through Azure Connection String (service defaults)

### **Npgsql Integration**
```csharp
services.AddDbContext<MediaspotDbContext>(options => 
	options.UseNpgsql(connectionString)
);
```

### **Entity Configurations**
- **AssetConfiguration** - Configures TPH (Table Per Hierarchy) strategy for Asset base class
- **AudioAssetConfiguration** - Discriminator-based configuration
- **VideoAssetConfiguration** - Discriminator-based configuration
- **TitleConfiguration** - Title table with unique constraint on Name
- **TranscodeJobConfiguration** - Job table with status indexes

### **Automatic Schema**
- Runs `Database.EnsureCreated()` on application startup
- No migration files needed for initial setup
- Can be upgraded to Migrations pattern later

---

## 🚀 Azure Aspire: Testability & Observability

### **Aspire Integration**
- **Components**:
  - `Mediaspot.Backend.TechnicalTest.Local.AppHost` - Orchestration project
  - `Mediaspot.Backend.TechnicalTest.Local.ServiceDefaults` - Shared service configuration

### **AppHost Configuration** - `AppHost.cs`
```csharp
// PostgreSQL resource
var postgres = builder.AddPostgres("postgres");
var database = postgres.AddDatabase("Database", databaseName: "MediaSpot");

// RabbitMQ resource
var rabbitmq = builder.AddRabbitMQ("Queuing");

// API service with dependencies
var webapi = builder.AddProject<Projects.Mediaspot_Api>("mediaspot-api")
	.WithReference(database)
	.WaitFor(database)
	.WithReference(rabbitmq)
	.WaitFor(rabbitmq);

// Worker service with dependencies
var worker = builder.AddProject<Projects.Mediaspot_Worker>("mediaspot-worker")
	.WithReference(rabbitmq)
	.WithReference(webapi)
	.WithReference(database)
	.WaitFor(rabbitmq)
	.WaitFor(webapi)
	.WaitFor(database);
```

### **Benefits**
1. **Testability**:
   - Containers spun up automatically for integration tests
   - No manual Docker setup required
   - Consistent test environment

2. **Observability**:
   - Service dependencies visualized in Aspire dashboard
   - Health checks integrated
   - Connection string management centralized
   - Environment variable injection automatic

3. **Local Development**:
   - Single command to launch entire system
   - Database and message queue auto-provisioned
   - Service URLs discovered automatically

4. **Service Defaults**:
   - Health check endpoints configured
   - Logging integrated
   - Telemetry ready (OpenTelemetry foundation)

---

## 🧪 Testing

### **Unit Tests**
- **Location**: `tests/Mediaspot.UnitTests/`
- **Framework**: xUnit
- **Test Suites**:
  - `AssetTests` - Asset aggregate behavior
  - `TranscodeJobTests` - Job status transitions
  - `CreateAssetHandlerTests` - Asset creation use case
  - `RegisterMediaFileHandlerTests` - Media file registration
  - `UpdateMetadataHandlerTests` - Metadata update logic
  - `ArchiveAssetHandlerTests` - Archive operation validation

- **Coverage**:
  - Domain invariant validation
  - Command handler logic
  - Event publishing
  - Exception scenarios

### **Integration Tests**
- **Location**: `Mediaspot.IntegrationTests/`
- **Framework**: NUnit
- **Test Suites**:
  - `Titles/TitleCrudTests` - Full create/read/update flow via API
  - `Transcoding/TranscodeJobTests` - Job lifecycle via API
  - `AspireSingletonFeature` - Aspire container orchestration

- **Features**:
  - Database integration with PostgreSQL
  - API endpoint testing
  - RabbitMQ message flow (where applicable)
  - End-to-end scenarios

### **Test Patterns**
- Arrange-Act-Assert (AAA) pattern
- Domain event assertion
- Status transition validation
- Exception handling verification

---

## 📁 Project Structure

```
src/
├── Mediaspot.Domain/
│   ├── Assets/
│   │   ├── AudioAssets/          # AudioAsset sealed class
│   │   ├── VideoAssets/          # VideoAsset sealed class
│   │   ├── Events/               # Domain events
│   │   ├── ValueObjects/         # Duration, Metadata, FilePath, etc.
│   │   └── Asset.cs              # Abstract aggregate root
│   ├── Titles/
│   │   ├── ValueObjects/         # TitleName, TitleDescription, etc.
│   │   └── Title.cs              # Aggregate root
│   ├── Transcoding/
│   │   ├── Events/               # TranscodeJob* events
│   │   ├── Exceptions/           # InvalidTranscodeStatusException
│   │   ├── ValueObjects/         # Preset
│   │   └── TranscodeJob.cs       # Aggregate root
│   └── Common/
│       ├── AggregateRoot.cs      # Base class with event publishing
│       ├── Entity.cs
│       └── IDomainEvent.cs
│
├── Mediaspot.Application/
│   ├── Assets/
│   │   ├── Commands/Create/Audios/
│   │   ├── Commands/Create/Videos/
│   │   ├── Commands/RegisterMediaFile/
│   │   ├── Commands/UpdateMetadata/
│   │   ├── Commands/Archive/
│   │   ├── Queries/GetById/
│   │   └── IAssetRepository interface
│   ├── Titles/
│   │   ├── Commands/Create/
│   │   ├── Commands/Update/
│   │   ├── Queries/GetById/
│   │   ├── Queries/List/
│   │   ├── ITitleRepository interface
│   │   └── Exceptions/
│   ├── Transcoding/
│   │   ├── Commands/CreateJob/
│   │   ├── Commands/StartJob/
│   │   ├── Commands/CompleteJob/
│   │   ├── Commands/FailJob/
│   │   ├── Queries/GetJobById/
│   │   ├── EventHandlers/
│   │   ├── IAssetTranscoder interface
│   │   └── ITranscodeQueuePublisher interface
│   └── Common/
│       ├── IRepository.cs
│       ├── IUnitOfWork.cs
│       └── Exceptions/
│
├── Mediaspot.Infrastructure/
│   ├── Persistence/
│   │   ├── Assets/AssetRepository, AssetConfiguration
│   │   ├── Titles/TitleRepository, TitleConfiguration
│   │   ├── Transcoding/TranscodeJobRepository, TranscodeJobConfiguration
│   │   ├── MediaspotDbContext.cs
│   │   ├── UnitOfWork.cs
│   │   └── DependencyInjection.cs (AddPersistence)
│   ├── Queuing/
│   │   ├── RabbitMqConnectionProvider.cs
│   │   ├── Transcoding/RabbitMqTranscodeQueuePublisher.cs
│   │   └── DependencyInjection.cs (AddQueueing)
│   └── Transcoding/
│       ├── AssetTranscoderFactory.cs
│       ├── AudioAssetTranscoder.cs
│       └── VideoAssetTranscoder.cs
│
├── Mediaspot.Api/
│   ├── Program.cs
│   ├── Titles/
│   │   ├── CreateTitle/CreateTitleEndpoint, CreateTitleDto
│   │   ├── GetTitleById/GetTitleEndpoint, TitleDto
│   │   ├── UpdateTitle/UpdateTitleEndpoint, UpdateTitleDto
│   │   ├── ListTitles/ListTitlesEndpoint
│   │   └── EndpointsGroup.cs
│   ├── Assets/
│   │   ├── CreateAsset/CreateAudioAsset/...
│   │   ├── CreateAsset/CreateVideoAsset/...
│   │   ├── RegisterMediaFile/RegisterMediaFileEndpoint, RegisterMediaFileDto
│   │   ├── UpdateMetatdata/UpdateMetadataEndpoint, UpdateMetadataDto
│   │   ├── ArchiveAsset/ArchiveAssetEndpoint
│   │   ├── GetAssetById/GetAssetEndpoint
│   │   └── EndpointsGroup.cs
│   ├── TranscodeJobs/
│   │   ├── CreateJob/CreateTranscodeJobEndpoint, CreateTranscodeJobDto
│   │   ├── StartJob/StartTranscodeJobEndpoint
│   │   ├── CompleteJob/CompleteTranscodeJobEndpoint, CompleteTranscodeJobDto
│   │   ├── FailJob/FailTranscodeJobEndpoint, FailTranscodeJobDto
│   │   ├── GetJobById/GetTranscodeJobByIdEndpoint, TranscodeJobDto
│   │   └── EndpointsGroup.cs
│   ├── ExceptionHandling/
│   │   ├── GlobalExceptionHandler.cs
│   │   └── ExceptionMapper.cs
│   └── EndpointsMapper.cs
│
├── Mediaspot.Worker/
│   ├── Program.cs
│   ├── TranscodeWorker.cs
│   └── MediaSpotApiClient.cs
│
├── Mediaspot.Backend.TechnicalTest.Local.AppHost/
│   └── AppHost.cs (Aspire orchestration)
│
└── Mediaspot.Backend.TechnicalTest.Local.ServiceDefaults/
	└── Service defaults for Aspire

tests/
├── Mediaspot.UnitTests/
│   ├── AssetTests.cs
│   ├── TranscodeJobTests.cs
│   ├── CreateAssetHandlerTests.cs
│   ├── RegisterMediaFileHandlerTests.cs
│   ├── UpdateMetadataHandlerTests.cs
│   └── ArchiveAssetHandlerTests.cs
│
└── Mediaspot.IntegrationTests/
	├── Titles/TitleCrudTests.cs
	├── Transcoding/TranscodeJobTests.cs
	└── AspireSingletonFeature.cs
```

---

## 🎯 Key Improvements & Architectural Decisions

### **1. PostgreSQL Database Migration**
- ✅ **Npgsql EF Core Provider**: Production-grade PostgreSQL support
- ✅ **Benefits**: 
  - Open-source database option
  - Better JSON support than SQL Server
  - Stronger type system
  - Cost-effective for cloud deployments
  - Excellent performance characteristics

### **2. Azure Aspire for Development & Testing**
- ✅ **Container Orchestration**: Automatic Docker-based environment
- ✅ **Improved Testability**:
  - Integration tests run against real PostgreSQL instance
  - RabbitMQ broker provisioned automatically
  - No environment setup friction

- ✅ **Better Observability**:
  - Service dependencies visualized in dashboard
  - Health checks monitored
  - Logs aggregated
  - Connection strings managed centrally
  - Ready for OpenTelemetry integration

### **3. Type-Safe Abstract Asset Hierarchy**
- ✅ **Polymorphic Persistence**: Uses TPH (Table Per Hierarchy) pattern
- ✅ **Type-Safe Operations**: Compiler guarantees asset-specific operations
- ✅ **Extensible**: New asset types (ImageAsset, DocumentAsset) can be added easily
- ✅ **Domain Logic**: Asset-specific behavior encapsulated in sealed classes

### **4. Event-Driven Job Management**
- ✅ **Decoupled Systems**: Job creation triggered by domain events, not direct calls
- ✅ **Audit Trail**: All status changes recorded as events
- ✅ **Future-Proof**: Event store implementation straightforward
- ✅ **State Machine**: Enforced status transitions prevent invalid states

### **5. Comprehensive Exception Handling**
- ✅ **Custom Exceptions**: Specific exceptions for domain constraints
- ✅ **Global Exception Handler**: Consistent error responses
- ✅ **API Mapping**: Domain exceptions mapped to HTTP status codes
- ✅ **Debugging**: Detailed error messages with context

### **6. Queue-Based Processing**
- ✅ **Worker Pattern**: Separate Worker project for background processing
- ✅ **RabbitMQ Integration**: Message-based job distribution
- ✅ **Scalability**: Multiple workers can process jobs concurrently
- ✅ **Resilience**: Queue persistence provides durability

### **7. Clean Architecture**
- ✅ **Separation of Concerns**: Domain ≠ Application ≠ Infrastructure
- ✅ **Testability**: Each layer independently testable
- ✅ **Maintainability**: Changes in one layer don't cascade
- ✅ **Dependency Rule**: Dependencies flow inward only

---

## 🔧 How to Run

### **Prerequisites**
- .NET 10 SDK
- Docker (for Aspire containers) or local PostgreSQL + RabbitMQ
- Visual Studio 2026 (Community edition or higher)

### **Using Aspire (Recommended)**
```bash
# Start entire system (API, Worker, PostgreSQL, RabbitMQ)
dotnet run --project src/Mediaspot.Backend.TechnicalTest.Local.AppHost/

# Aspire dashboard available at: https://localhost:15000
# API available at: https://localhost:5001
```

### **Manual Setup (Without Aspire)**
```bash
# Set connection strings in appsettings.Development.json
# Then run API
dotnet run --project src/Mediaspot.Api/

# In another terminal, run Worker
dotnet run --project src/Mediaspot.Worker/
```

### **Running Tests**
```bash
# Unit tests
dotnet test tests/Mediaspot.UnitTests/

# Integration tests (requires Aspire or manual DB setup)
dotnet test Mediaspot.IntegrationTests/
```

---

## 📝 API Usage Examples

### **Create Title**
```http
POST /titles
Content-Type: application/json

{
  "name": "Inception",
  "description": "A mind-bending thriller",
  "releaseDate": "2010-07-16",
  "type": "Movie"
}
```

### **Create Audio Asset**
```http
POST /assets/audio
Content-Type: application/json

{
  "externalId": "audio-001",
  "metadata": {
	"title": "Podcast Episode 1",
	"description": "First episode",
	"additionalData": null
  },
  "duration": "00:45:30",
  "bitrate": 128,
  "sampleRate": 44100,
  "channels": 2
}
```

### **Create Video Asset**
```http
POST /assets/video
Content-Type: application/json

{
  "externalId": "video-001",
  "metadata": {
	"title": "Movie Trailer",
	"description": "Official trailer",
	"additionalData": null
  },
  "duration": "00:02:30",
  "resolution": "1920x1080",
  "frameRate": 24.0,
  "codec": "H.264"
}
```

### **Register Media File**
```http
POST /assets/{assetId}/media-files
Content-Type: application/json

{
  "filePath": "/media/movie.mp4",
  "durationSeconds": 150
}
```

### **Create Transcode Job**
```http
POST /transcode-jobs
Content-Type: application/json

{
  "assetId": "asset-guid",
  "mediaFileId": "media-file-guid",
  "preset": "HD_720p"
}
Response: { "jobId": "job-guid", "status": "Pending" }
```

### **Start Job**
```http
PATCH /transcode-jobs/{jobId}/start
```

### **Complete Job**
```http
PATCH /transcode-jobs/{jobId}/complete
Content-Type: application/json

{
  "completedAt": "2024-01-15T10:30:00Z"
}
```

---

## 🎓 Design Patterns Used

| Pattern | Location | Purpose |
|---------|----------|---------|
| **Aggregate Root** | Domain/*.cs | Boundary definition, event publishing |
| **Value Object** | Domain/*/ValueObjects | Type-safe, immutable properties |
| **Repository** | Application interfaces, Infrastructure implementations | Data access abstraction |
| **Factory** | AssetTranscoderFactory | Type-safe object creation |
| **Command Handler** | Application/*/Commands | CQRS command processing |
| **Query Handler** | Application/*/Queries | CQRS query processing |
| **Event Handler** | Application/*/EventHandlers | Domain event subscription |
| **Dependency Injection** | DependencyInjection extension methods | Modular service registration |
| **Unit of Work** | Infrastructure/UnitOfWork | Transaction management |
| **Exception Handling** | GlobalExceptionHandler | Centralized error responses |
| **Worker Pattern** | Mediaspot.Worker | Background job processing |

---

## 🐛 Known Notes/Future Enhancements

1. **Event Sourcing**: Current event system ready for event store implementation
2. **Saga Pattern**: For distributed workflows between services
3. **Soft Deletes**: Could add soft delete pattern for archives
4. **Audit Logging**: Detailed change tracking per entity
5. **Message Deadletter Queue**: Handle failed RabbitMQ messages
6. **Rate Limiting**: Transcode job submission limits
7. **Webhook Notifications**: Notify external systems of job completion
8. **Metrics & Alerting**: OpenTelemetry integration for performance monitoring
9. **API Versioning**: Prepare for v2 API with backward compatibility
10. **Caching**: Implement caching for frequently accessed titles/assets

---

## 📊 Summary Statistics

| Metric | Count |
|--------|-------|
| Domain Entities/Value Objects | 15+ |
| Commands | 7 |
| Queries | 4 |
| Domain Events | 10+ |
| API Endpoints | 15+ |
| Repositories | 3 |
| Use Case Handlers | 11+ |
| Unit Tests | 6+ |
| Integration Tests | 3+ |
| Service Projects | 4 (API, Worker, AppHost, ServiceDefaults) |

---

## ✨ Highlights

✅ **All Basic Tasks Completed** - Title entity with business rules and full CRUD API  
✅ **All Middle Tasks Completed** - Event-driven TranscodeJob model with worker implementation  
✅ **All Advanced Tasks Completed** - Abstract Asset with AudioAsset/VideoAsset specialization  
✅ **Bonus Features Implemented** - Archive/de-archive with side effects, queue-based processing  
✅ **PostgreSQL Integration** - Production-ready database with Npgsql  
✅ **Azure Aspire** - Development environment with observability and testability  
✅ **Comprehensive Tests** - Unit and integration test suites  
✅ **Clean Architecture** - Following DDD and CQRS patterns  
✅ **Production-Ready** - Exception handling, dependency injection, service defaults  

---

_Generated for the Mediaspot Backend Technical Test implementation_
