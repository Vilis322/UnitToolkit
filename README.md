# UnitToolkit

A comprehensive multi-tool utility application in **C#/.NET 9** with **three UIs**:
- **Console CLI** (text menu)
- **Avalonia Desktop** (dark-themed, card-based layout)
- **MAUI Android** (native mobile app)

All domain logic lives in `UnitToolkit.Core` and is shared across all UIs through `UnitToolkit.Presentation` ViewModels (DRY principle).

---

## Features (5 tools)

1) **Units Converter** — convert:
    - Temperature: `C` ↔ `F` ↔ `K`
    - Length: `m` ↔ `cm` ↔ `km`
    - Mass: `kg` ↔ `g` ↔ `lb`
2) **Currency Exchange** — real-time currency conversion with live exchange rates from API.
3) **Password / Random**
    - Password generator (cryptographically strong RNG), options: uppercase, digits, symbols, length.
    - Random integer in a range (cryptographically strong).
4) **BMI Calculator**
    - Height in meters (e.g., `1.70`) or centimeters (e.g., `170`), weight in kg.
    - Returns BMI value, WHO category, and a short advice.
5) **Data Size Converter**
    - SI: `B`, `KB`, `MB`, `GB`, `TB` (10^3)
    - IEC: `KiB`, `MiB`, `GiB`, `TiB` (2^10)

---

## Architecture
```
UnitToolkit/
├─ UnitToolkit.sln
├─ README.md
├─ Makefile                             # Build automation
├─ .gitignore
├─ src/
│  ├─ UnitToolkit.Core/                 # Domain logic (UI-free services)
│  │  ├─ UnitToolkit.Core.csproj
│  │  └─ Services/
│  │     ├─ UnitConverter.cs
│  │     ├─ CurrencyExchangeService.cs
│  │     ├─ PasswordGenerator.cs
│  │     ├─ BmiCalculator.cs
│  │     └─ DataSizeConverter.cs
│  ├─ UnitToolkit.Presentation/         # Shared ViewModels (MVVM)
│  │  ├─ UnitToolkit.Presentation.csproj
│  │  └─ ViewModels/
│  │     ├─ ViewModelBase.cs
│  │     ├─ MainViewModel.cs
│  │     ├─ UnitsViewModel.cs
│  │     ├─ CurrencyViewModel.cs
│  │     ├─ PasswordViewModel.cs
│  │     ├─ BmiViewModel.cs
│  │     └─ DataSizeViewModel.cs
│  ├─ UnitToolkit.Cli/                  # Console UI
│  │  ├─ UnitToolkit.Cli.csproj
│  │  ├─ Common/
│  │  │  └─ Input.cs
│  │  └─ Program.cs
│  ├─ UnitToolkit.Desktop/              # Avalonia Desktop UI
│  │  ├─ UnitToolkit.Desktop.csproj
│  │  ├─ App.axaml
│  │  ├─ App.axaml.cs
│  │  └─ Views/
│  │     ├─ MainWindow.axaml            # Single window with view switching
│  │     └─ MainWindow.axaml.cs
│  └─ UnitToolkit.Mobile/               # MAUI Android UI
│     ├─ UnitToolkit.Mobile.csproj      # Targets: net9.0-android only
│     ├─ MauiProgram.cs                 # DI container & logging setup
│     ├─ App.xaml / App.xaml.cs         # Global exception handling
│     ├─ AppShell.xaml                  # Navigation shell
│     ├─ MainPage.xaml                  # Dashboard with tool cards
│     ├─ Pages/
│     │  ├─ UnitsPage.xaml
│     │  ├─ CurrencyPage.xaml
│     │  ├─ PasswordPage.xaml
│     │  ├─ BmiPage.xaml
│     │  └─ DataSizePage.xaml
│     ├─ Resources/Styles/
│     │  ├─ AppColors.xaml              # Dark theme colors
│     │  └─ AppStyles.xaml              # Reusable styles
│     └─ Converters/
│        └─ StringNotEmptyConverter.cs
└─ tests/
   └─ UnitToolkit.Tests/                # xUnit tests for all services
      ├─ UnitToolkit.Tests.csproj
      ├─ UnitConverterTests.cs
      ├─ CurrencyExchangeServiceTests.cs
      ├─ PasswordGeneratorTests.cs
      ├─ BmiCalculatorTests.cs
      └─ DataSizeConverterTests.cs
```

---

## Requirements

- **.NET SDK 9** (includes runtime 9)
- **For Desktop**: macOS / Windows / Linux (Avalonia is cross-platform)
- **For Android**: Android SDK, Android emulator or physical device
- Optional: JetBrains Rider or Visual Studio with MAUI workload

Check versions:
```bash
dotnet --info
dotnet workload list  # Verify MAUI is installed
```

---

## Build & Run

### Using Make (recommended)

View all available commands:
```bash
make help
```

#### Desktop (Avalonia)
```bash
make run-desktop
```

#### CLI
```bash
make run-cli
```

#### Android
```bash
# Build for Android
make android-build

# Run on connected emulator/device
make android-run

# Stream Android logs
make android-log
```

#### Tests
```bash
make test
```

#### Build all projects
```bash
make build
```

### Without Make

#### Build everything
```bash
dotnet build
```

#### Run CLI
```bash
dotnet run --project src/UnitToolkit.Cli/UnitToolkit.Cli.csproj
```

#### Run Desktop (Avalonia)
```bash
dotnet run --project src/UnitToolkit.Desktop/UnitToolkit.Desktop.csproj
```

#### Run Android
```bash
# Build for Android
dotnet build src/UnitToolkit.Mobile/UnitToolkit.Mobile.csproj -f net9.0-android

# Build and run on emulator/device
dotnet build src/UnitToolkit.Mobile/UnitToolkit.Mobile.csproj -f net9.0-android -t:Run

# View logs (requires adb)
adb logcat -s "UnitToolkit:*" "mono-stdout:*" "dotnet:*"
```

#### Tests
```bash
dotnet test tests/UnitToolkit.Tests/UnitToolkit.Tests.csproj
```

---

## Android App Features

The Android app provides:
- **Dark Theme**: Card-based UI matching the desktop Avalonia theme
- **Navigation**: Dashboard with 5 tool cards, each opening a dedicated page
- **Dependency Injection**: Full DI support with Microsoft.Extensions
- **Logging**: Comprehensive logging to console/logcat for debugging
- **Exception Handling**: Global handlers for unhandled exceptions and unobserved tasks
- **Shared Logic**: Uses the same ViewModels and Core services as desktop

### Android UI Structure

1. **MainPage**: Dashboard with 5 tool cards
2. **UnitsPage**: Unit converter with type selection
3. **CurrencyPage**: Real-time currency exchange with loading indicator
4. **PasswordPage**: Password generator and random integer generator
5. **BmiPage**: BMI calculator with health advice
6. **DataSizePage**: Data size converter

All pages include:
- Back navigation
- Input validation with error display (red text)
- Results display (green text)
- Logging of user actions

---

## Project Structure

### Layer Architecture
```
┌─────────────────────────────────────┐
│  UI Layer (3 implementations)       │
│  - Desktop (Avalonia)               │
│  - Mobile (MAUI Android)            │
│  - CLI (Console)                    │
├─────────────────────────────────────┤
│  Presentation Layer                 │
│  - Shared ViewModels (MVVM)         │
│  - UI-agnostic business logic       │
├─────────────────────────────────────┤
│  Core Layer                         │
│  - Domain services                  │
│  - Pure business logic              │
│  - No UI dependencies               │
└─────────────────────────────────────┘
```

### Key Design Principles

1. **Separation of Concerns**: Core, Presentation, and UI layers
2. **DRY (Don't Repeat Yourself)**: Shared ViewModels across Desktop and Mobile
3. **Dependency Injection**: Constructor injection throughout
4. **MVVM Pattern**: Clean separation of UI and logic
5. **Testability**: Core services fully unit tested