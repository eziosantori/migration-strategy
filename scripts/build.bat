@REM @echo off
@REM REM Build the solution
@REM call dotnet build MigrationStrategy.sln
@REM REM Run tests with coverage
@REM call dotnet test MigrationStrategy.sln --collect:"XPlat Code Coverage"


@echo off
REM Migration Service - Build and Test Script
REM This script builds the solution and runs tests with code coverage

echo ========================================
echo Migration Service - Build and Test
echo ========================================

echo.
echo [1/4] Cleaning solution...
dotnet clean -v q
if %errorlevel% neq 0 (
    echo ERROR: Failed to clean solution
    exit /b 1
)

echo [2/4] Restoring NuGet packages...
dotnet restore -v q
if %errorlevel% neq 0 (
    echo ERROR: Failed to restore packages
    exit /b 1
)

echo [3/4] Building solution...
dotnet build --no-restore -v q
if %errorlevel% neq 0 (
    echo ERROR: Failed to build solution
    exit /b 1
)

echo [4/4] Running tests with code coverage...
dotnet test --no-build --verbosity normal --collect:"XPlat Code Coverage"
if %errorlevel% neq 0 (
    echo ERROR: Tests failed
    exit /b 1
)

echo.
echo ========================================
echo Build and test completed successfully!
echo ========================================
echo.
echo To view coverage report:
echo - Check the TestResults folder for coverage files
echo - Use coverage tools like ReportGenerator or Visual Studio

pause

