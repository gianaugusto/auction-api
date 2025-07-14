# AutoAuction

AutoAuction is a sample project demonstrating a clean architecture approach for a simple auction system.

## Project Structure

- **src/AutoAuction.API**: ASP.NET Core presentation layer
- **src/AutoAuction.Application**: Application services and use cases
- **src/AutoAuction.Domain**: Domain entities, aggregates, and business rules
- **src/AutoAuction.Infrastructure**: Technical implementations (e.g., database, integrations)
- **src/AutoAuction.CrossCutting**: Cross-cutting concerns (logging, authentication, validation)
- **tests/AutoAuction.UnitTests**: Unit tests for domain and application layers
- **tests/AutoAuction.IntegrationTests**: Integration tests for the API

## Getting Started

1. Clone the repository
2. Open the solution in Visual Studio or your preferred IDE
3. Build and run the solution

## Running Tests

To run the unit tests, use the following command:

```bash
dotnet test tests/AutoAuction.UnitTests/AutoAuction.UnitTests.csproj
```

To run the integration tests, use the following command:

```bash
dotnet test tests/AutoAuction.IntegrationTests/AutoAuction.IntegrationTests.csproj
```

## Docker and Docker Compose

This project includes Docker and Docker Compose configurations for easy setup and testing.

### Services

- **api**: The ASP.NET Core API service
- **db**: PostgreSQL database service
- **tests**: Unit tests service
- **integration-tests**: Integration tests service

### Running with Docker Compose

1. Ensure Docker and Docker Compose are installed on your machine
2. Navigate to the project root directory
3. Run the following command to start all services:

```bash
docker-compose up --build
```

This will build and start the API, database, and test services.

### Running Tests with Docker

To run the unit tests:

```bash
docker-compose run tests
```

To run the integration tests:

```bash
docker-compose run integration-tests
```

## Contributing

Contributions are welcome! Please open an issue or submit a pull request.

## License

This project is licensed under the MIT License.
