# Users Microservice

This microservice handles user management, profiles, and authentication events within the distributed system. It is built following Clean Architecture principles to ensure high maintainability, testability, and decoupling.

## Architecture Layering

The solution is organized into the following layers:

1.  **Core.Domain**: Business entities and core interfaces. Contains the definition of the User entity and data access contracts.
2.  **Core.Application**: Use cases and business logic. It handles the orchestration of data between the UI and the persistence layer.
3.  **Infrastructure**: Implementation details. Includes MongoDB persistence and RabbitMQ messaging client.
4.  **API**: Entry point of the application. Handles HTTP requests, middleware, and dependency injection.

## Tech Stack

- **Framework**: .NET 9.0 (ASP.NET Core)
- **Database**: MongoDB (NoSQL) for high-performance user profile storage.
- **Messaging**: RabbitMQ as the message broker for event-driven synchronization.
- **Documentation**: Scalar API Reference (OpenAPI 3.1) for modern and interactive documentation.
- **Validation**: FluentValidation for robust input data integrity.
- **Inversion of Control**: Built-in .NET Dependency Injection.

## Features

- **Global Exception Handling**: Custom middleware to ensure consistent error responses.
- **Event-Driven Communication**: Automatically publishes a `UserCreated` event to RabbitMQ upon new registrations.
- **Clean Architecture**: Strict separation of concerns to support independent evolution of layers.
- **High Performance**: Optimized MongoDB drivers and asynchronous processing.

## Getting Started

### Prerequisites

- Docker and Docker Compose
- .NET 9 SDK

### Local Infrastructure

Start the required services using Docker Compose:

```bash
docker compose up -d
```

This will spin up:

- MongoDB at `localhost:27017`
- RabbitMQ at `localhost:5672` (Management UI at `http://localhost:15672`)

### Running the Application

Execute the following command in the root directory:

```bash
dotnet run
```

The service will be available at `http://localhost:5057`.

## Documentation

Interactive API documentation can be accessed at:

- **Scalar**: `http://localhost:5057/scalar/v1`

## API Endpoints

- `GET /api/users`: Retrieve all users.
- `GET /api/users/{id}`: Retrieve a specific user by GUID.
- `POST /api/users`: Create a new user (triggers validation and events).
- `GET /api/users/health`: Service health check.
- `GET /api/users/status`: Service operational status.
