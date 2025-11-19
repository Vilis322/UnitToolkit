PROJECT_DESKTOP=src/UnitToolkit.Desktop/UnitToolkit.Desktop.csproj
PROJECT_CLI=src/UnitToolkit.Cli/UnitToolkit.Cli.csproj
PROJECT_TESTS=tests/UnitToolkit.Tests/UnitToolkit.Tests.csproj

.PHONY: run-desktop run-cli test build clean

run-desktop:
	dotnet run --project $(PROJECT_DESKTOP)

run-cli:
	dotnet run --project $(PROJECT_CLI)

test:
	dotnet test $(PROJECT_TESTS)

build:
	dotnet build

clean:
	dotnet clean
