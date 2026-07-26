# Solution Overview

## General Improvements

1. The Web API project follows a **screaming architecture**, organized by resource (entity).
2. Global exception handling with **ProblemDetails** provides a standardized error‑handling experience.  
   Stack traces are only returned in development mode.
3. The solution runs on **.NET 10**, benefiting from performance improvements.
4. All entities use **Guid v7** for improved performance and keyset pagination.
5. A **generic repository pattern** is implemented, with a base repository for shared operations.
6. A real **ACID-compliant database** is used via PostgreSQL instead of an in‑memory provider.
7. **Observability** is integrated using Microsoft Aspire.
8. The solution runs locally using **Aspire orchestration**, enabling containerization of the database, Web API, queue, and worker.
9. **Integration test support** is added using Aspire.
10. A **RabbitMQ queue** is integrated into the solution.

---

## Task 1 — Title CRUD

1. Create a title with nullable properties.
2. List titles using **keyset pagination** (Guid v7) and a configurable page size.
3. Update a title with nullable properties.  
   If a property is `null` in the DTO, it is not updated—preventing accidental data loss.
4. Retrieve a title by ID.
5. Title name search is **case-insensitive** using database collation rather than string transformations.
6. Production-oriented improvement: return DTOs instead of raw GUIDs.  
   Contracts are preferable for consumer stability, but this project is a demo.

---

## Task 2 + Part of Task 3 — Transcode Job State Management

1. The `Preset` property is now a **value object**, reflecting its domain meaning and behavior.
2. Several new properties were added, notably:  
   - **Failure Reason** (nullable), because failure is domain information.  
   - **Version**, enabling concurrency control since job state transitions are business logic.
3. Job state transitions are performed **exclusively through entity methods**.
4. Five use cases + endpoints were added:  
   **Create (Pending) / Get / Start / Fail / Complete**.
5. State changes raise **domain events**.  
   Job creation publishes a message (currently `jobId`) to RabbitMQ.  
   A DLQ can be added for failed job events.
6. Messages are consumed by a worker—details below.

---

## Task 3 + Worker

1. Added an endpoint for creating **video assets**.
2. Added an endpoint for creating **audio assets**.
3. Improvement:  
   - `GET /assets/{id}` → returns a **base asset DTO** with a type discriminator.  
   - `GET /assets/video/{id}` → returns a **video asset DTO**.  
   - `GET /assets/audio/{id}` → returns an **audio asset DTO**.  
   Current implementation supports polymorphic deserialization via discriminators.
4. Improvement: all asset properties should be **value objects**.  
   Logic is implemented for resolution; similar logic applies to fields like `bitRate`.
5. Worker architecture:  
   In production, the worker runs in its own container for isolation and custom VM specs.  
   It:  
   - Calls the Web API to update job status.  
   - Subscribes to RabbitMQ for new jobs.
6. If calling the Web API is not ideal, the Application layer can be exported as a **NuGet package** for direct worker consumption.

### Improvement++
Multiple workers per asset type.  
One queue per asset type.  
This removes polymorphism at the worker level, reduces cost, and improves throughput.  
Video and audio transcoding do not require identical resource allocation.

---

## Archiving Files

The goal is to archive media files.  
When the archiving domain event is raised (containing file paths/URLs), the handler tags all files with `archive=true`.  
A file manager implementation (e.g., S3) updates file tags, and lifecycle policies move files to **Glacier Deep Archive**.

```hcl
resource "aws_s3_bucket_lifecycle_configuration" "archive_by_tag" {
  bucket = aws_s3_bucket.data.id

  rule {
    id     = "archive-by-tag-to-deep-archive"
    status = "Enabled"

    filter {
      tag {
        key   = "archive"
        value = "true"
      }
    }

    transition {
      days          = 0
      storage_class = "DEEP_ARCHIVE"
    }
  }
}
```

De‑archiving follows the same pattern with reversed transitions.

---

## Notes

1. For this proof of concept, the focus is on **integration tests** rather than unit tests.  
   They validate more functionality and component interaction with fewer test cases.

---

## How to Run the Project

1. Update Visual Studio to the latest version.
2. Ensure Docker is running.
3. Run the **AppHost**.  
   The first launch may take time to pull required images (PostgreSQL, RabbitMQ).