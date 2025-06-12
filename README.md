# Migration Service

A robust migration service for Happy Time Inc. products and categories, built with C# .NET 8 using Test-Driven Development (TDD).

## Features

- **Product and Category Migration**: Support for both single and bulk migrations
- **Dependency Management**: Automatic handling of product-category relationships
- **Error Handling**: Comprehensive error management with rollback capabilities
- **Interactive Console**: User-friendly command-line interface
- **Extensible Architecture**: Clean architecture with SOLID principles

## Migration Types

- **COPY_SINGLE**: Copy individual products or categories
- **MOVE_SINGLE**: Move individual products or categories
- **COPY_MANY**: Copy with all dependencies (products + their categories)
- **MOVE_MANY**: Move with all dependencies (products + their categories)

## Getting Started

### Prerequisites

- .NET 8 SDK
- Visual Studio 2022 or Visual Studio Code

### Building the Project

```bash
dotnet build
```

### Running Tests

```bash
# Run all tests
dotnet test

# Run tests with code coverage
dotnet test --collect:"XPlat Code Coverage" --settings coverlet.runsettings

# Using the provided scripts
# Windows Batch
.\scripts\build-and-test.bat

# PowerShell (recommended)
.\scripts\build-and-test.ps1

# PowerShell with options
.\scripts\build-and-test.ps1 -SkipBuild      # Skip build, run tests only
.\scripts\build-and-test.ps1 -NoCoverage    # Run tests without coverage
```

### Running the Console Application

```bash
dotnet run --project src/MigrationService.Console
```

## Project Structure

```
migration-strategy-net/
├── src/
│   ├── MigrationService.Core/          # Core business logic
│   │   ├── Models/                     # Domain models
│   │   ├── Enums/                      # Enumerations
│   │   ├── Interfaces/                 # Contracts
│   │   ├── Services/                   # Business services
│   │   └── Exceptions/                 # Custom exceptions
│   └── MigrationService.Console/       # Console application
└── tests/
    └── MigrationService.Tests/         # Test suite
        ├── Unit/                       # Unit tests
        └── Integration/                # Integration tests
```

## Architecture

The project follows Clean Architecture principles with:

- **Domain Models**: Product, Category, Group
- **Repository Pattern**: Abstracted persistence layer
- **Service Layer**: Business logic implementation
- **Console Interface**: User interaction layer

## Development Approach

This project is developed using **Test-Driven Development (TDD)**:

1. **Red**: Write failing tests first
2. **Green**: Implement minimum code to pass tests
3. **Refactor**: Improve code while keeping tests green

## Testing

The project uses a comprehensive testing strategy:

### Testing Framework
- **xUnit**: Primary testing framework for unit and integration tests
- **Moq**: Mocking framework for isolating dependencies
- **FluentAssertions**: Provides readable and expressive assertions
- **Coverlet**: Code coverage analysis and reporting

### Test Structure
- **Unit Tests**: Located in `tests/MigrationService.Tests/Unit/`
- **Integration Tests**: Located in `tests/MigrationService.Tests/Integration/`
- **Test Verification**: Framework verification tests ensure proper setup

### Coverage Requirements
- **Minimum Coverage**: 90% for new functionality
- **Coverage Reports**: Generated in multiple formats (OpenCover, Cobertura, HTML)
- **Coverage Settings**: Configured via `coverlet.runsettings`

### Running Tests
```bash
# Quick test run
dotnet test

# With coverage
dotnet test --collect:"XPlat Code Coverage"

# Using scripts
.\scripts\build-and-test.ps1
```

## Contributing

1. Follow TDD approach
2. Maintain test coverage > 90%
3. Document public APIs
4. Follow established coding standards

## License

This project is developed for Happy Time Inc.
