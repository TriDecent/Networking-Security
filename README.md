# DataEncryptionApp and AdvancedNumbersCalculator

## Overview

This repository contains two main applications:

1. **DataEncryptionApp**: An application focused on encrypting and decrypting data from string input or files.
2. **AdvancedNumbersCalculator**: An application for performing advanced mathematical calculations (better suited as a library, the app is just for showcase).
3. **HashCalculator**: An application for calculating various hash values from text, hex and files.

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

For optimal performance, use an encryption key of 1024 bits or higher (better use the default 4096 bits). Using smaller keys may result in incorrect decryption outcomes. The public or private key must be in the format (n, e) or (n, d) respectively.

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

# HashCalculator

## Structure

- `Enums/`: Contains enumeration types used by the application.
- `HashGenerators/`: Contains hash generation logic.
- `Utils/`: Contains utility functions.
- `Program.cs`: The main entry point of the application.

## How to Build and Run

1. **Build the Application**:

   ```sh
   dotnet build HashCalculator/HashCalculator.csproj
   ```

2. **Run the Application**:

   ```sh
   dotnet run --project HashCalculator/HashCalculator.csproj
   ```

### Note on Using RSA Encryption

HashCalculator is a Windows Forms application. Ensure you have a compatible environment to run Windows Forms applications.