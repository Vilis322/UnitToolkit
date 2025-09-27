# UnitToolkit

A small multi-tool study app in **C#** with **two UIs**:
- **Console CLI** (text menu)
- **Avalonia Desktop** (5 tabs)

All domain logic lives in `UnitToolkit.Core` and is reused by both UIs (DRY).

---

## Features (5 tools)

1) **Units Converter** — convert:
    - Temperature: `C` ↔ `F` ↔ `K`
    - Length: `m` ↔ `cm` ↔ `km`
    - Mass: `kg` ↔ `g` ↔ `lb`
2) **Currency Calculator** — amount × rate (rate is entered manually, no network).
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
├─ .gitignore
├─ src/
│  ├─ UnitToolkit.Core/                 # Domain logic (no UI)
│  │  ├─ UnitToolkit.Core.csproj
│  │  └─ Services/
│  │     ├─ UnitConverter.cs
│  │     ├─ CurrencyCalculator.cs
│  │     ├─ PasswordGenerator.cs
│  │     ├─ BmiCalculator.cs
│  │     └─ DataSizeConverter.cs
│  ├─ UnitToolkit.Cli/                  # Console UI
│  │  ├─ UnitToolkit.Cli.csproj
│  │  ├─ Common/
│  │  │  └─ Input.cs
│  │  └─ Program.cs
│  └─ UnitToolkit.Desktop/              # Avalonia Desktop UI
│     ├─ UnitToolkit.Desktop.csproj
│     ├─ App.axaml
│     ├─ App.axaml.cs
│     └─ Views/
│        ├─ MainWindow.axaml            # 5 tabs (Units, Currency, Password/Random, BMI, Data Size)
│        └─ MainWindow.axaml.cs         # thin handlers calling Core services
└─ tests/
   └─ UnitToolkit.Tests/                # xUnit tests for all 5 services
      ├─ UnitToolkit.Tests.csproj
      ├─ UnitConverterTests.cs
      ├─ CurrencyCalculatorTests.cs
      ├─ PasswordGeneratorTests.cs
      ├─ BmiCalculatorTests.cs
      └─ DataSizeConverterTests.cs
```

---

## Requirements

- **.NET SDK 9** (includes runtime 9)
- macOS / Windows / Linux (Avalonia is cross-platform)
- Optional: JetBrains Rider (Avalonia plugin recommended)

Check versions:
```bash
dotnet --info
```

---

## Build & Run

### Build everything
```bash
dotnet build
```

### Run CLI
```bash
dotnet run --project src/UnitToolkit.Cli
```

### Run Desktop (Avalonia)
```bash
dotnet run --project src/UnitToolkit.Desktop
```

### Tests
```bash
dotnet test
```