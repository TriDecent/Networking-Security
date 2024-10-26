# DataEncryptionApp and AdvancedNumbersCalculator

## Overview

This repository contains two main applications:

1. **DataEncryptionApp**: An application focused on data encryption and decryption.
2. **AdvancedNumbersCalculator**: An application for performing advanced mathematical calculations.

## DataEncryptionApp

### Structure

- `App/`: Contains the main application logic.
- `DataAccess/`: Handles data access operations.
- `DataEncryption/`: Contains encryption and decryption logic.
- `DataFiles/`: Stores data files used by the application.
- `UI/`: Contains the user interface components.
- `Program.cs`: The main entry point of the application.

### How to Build and Run

1. **Build the Application**:

   ```sh
   dotnet build DataEncryptionApp/DataEncryptionApp.csproj
   ```

2. **Run the Application**:
   ```sh
   dotnet run --project DataEncryptionApp/DataEncryptionApp.csproj
   ```

### Note on Using RSA Encryption

For optimal performance, use an encryption key of 1024 bits or higher (better use the default 4096 bits). Using smaller keys may result in incorrect decryption outcomes.

## AdvancedNumbersCalculator

### Structure

- `App/`: Contains the main application logic.
- `LogicalMath/`: Contains mathematical logic and operations.
- `UI/`: Contains the user interface components.
- `Utilities/`: Contains utility functions.
- `Program.cs`: The main entry point of the application.

### How to Build and Run

1. **Build the Application**:

   ```sh
   dotnet build AdvancedNumbersCalculator/AdvancedNumbersCalculator.csproj
   ```

2. **Run the Application**:
   ```sh
   dotnet run --project AdvancedNumbersCalculator/AdvancedNumbersCalculator.csproj
   ```
