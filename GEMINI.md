# Project Overview

This is a microservices-based e-commerce platform called "DreamsShop". The platform allows users to buy and sell "dreams". The architecture consists of a .NET-based backend, an Angular frontend, and several backing services, all containerized with Docker.

The main components are:

*   **`Business` Service**: A .NET service that implements the core business logic of the application, including managing dreams, categories, and orders. It follows a Clean Architecture pattern.
*   **`Identity` Service**: A .NET service responsible for user authentication and authorization. It distinguishes between "Consumer" and "Producer" user roles. It also follows a Clean Architecture pattern.
*   **`Client`**: An Angular single-page application that provides the user interface for the platform.
*   **`nginx`**: A reverse proxy that routes requests to the appropriate backend service.
*   **`postgres`**: A PostgreSQL database for data persistence.
*   **`minio`**: An S3-compatible object storage service, likely for storing dream-related media.
*   **`redis`**: A Redis in-memory data store, likely for caching or session management.

The entire application is orchestrated using `docker-compose.yml`.

# Building and Running

The project is containerized and can be built and run using Docker Compose.

**Build and run the entire application:**

```bash
docker-compose up --build
```

**Run the application (without rebuilding):**

```bash
docker-compose up
```

**Stop the application:**

```bash
docker-compose down
```

**Frontend Development:**

The frontend is an Angular application. To run the frontend in a local development environment for easier debugging and faster reloading, navigate to the `src/Client` directory and run:

```bash
npm install
ng serve
```

The frontend will be available at `http://localhost:4200/`.

**Backend Development:**

The backend services are .NET applications. To run them locally, you will need the .NET SDK. You can run the services by opening the `.sln` files in `src/Business` and `src/Idenitity` with a .NET-compatible IDE like Visual Studio or JetBrains Rider, or by using the `dotnet run` command.

# Development Conventions

*   **Backend**: The backend services follow a Clean Architecture pattern, separating concerns into `Domain`, `Application`, `Infrastructure`, and `Presentation` layers.
*   **Frontend**: The frontend is an Angular application and follows the standard Angular project structure and conventions.
*   **CI/CD**: The presence of a `Jenkinsfile` indicates that the project uses Jenkins for continuous integration and deployment.
*   **API Documentation**: The `DreamsShop_Bruno` directory contains a Bruno collection for API testing and documentation. This can be used to understand and interact with the API endpoints.
