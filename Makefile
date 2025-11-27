PROJECT_DESKTOP=src/UnitToolkit.Desktop/UnitToolkit.Desktop.csproj
PROJECT_CLI=src/UnitToolkit.Cli/UnitToolkit.Cli.csproj
PROJECT_TESTS=tests/UnitToolkit.Tests/UnitToolkit.Tests.csproj
PROJECT_MOBILE=src/UnitToolkit.Mobile/UnitToolkit.Mobile.csproj

.PHONY: help run-desktop run-cli test build clean mobile-android-build mobile-android-run mobile-android-log

help:
	@echo "Available targets:"
	@echo "  run-desktop           - Run the Avalonia desktop application"
	@echo "  run-cli               - Run the CLI application"
	@echo "  test                  - Run all unit tests"
	@echo "  build                 - Build all projects in the solution"
	@echo "  clean                 - Clean all build artifacts"
	@echo "  android-build  - Build the Android MAUI application"
	@echo "  android-run    - Build and run the Android app on emulator/device"
	@echo "  android-log    - Stream Android logs filtered by UnitToolkit"

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

android-build:
	dotnet build $(PROJECT_MOBILE) -f net9.0-android

android-run:
	dotnet build $(PROJECT_MOBILE) -f net9.0-android -t:Run

android-log:
	adb logcat -s "UnitToolkit:*" "mono-stdout:*" "dotnet:*"
