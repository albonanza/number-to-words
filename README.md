# Number-to-Words Converter Application

## Overview

The **Number-to-Words Converter** is an ASP.NET Core MVC application that converts numeric currency amounts into their corresponding English words. For example, it converts `123.45` into:

```
ONE HUNDRED AND TWENTY-THREE DOLLARS AND FORTY-FIVE CENTS
```

This application is designed with extensibility in mind, allowing for easy addition of multiple languages and currencies in the future. It leverages Dependency Injection to manage services and configurations, promoting a modular and maintainable codebase.

---

## Table of Contents

- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Building and Running](#building-and-running)
- [Usage](#usage)
- [Project Structure](#project-structure)
- [Dependency Injection](#dependency-injection)
- [Customisation](#customisation)
- [Contact](#contact)

---

## Prerequisites

Before you begin, please ensure you have the following installed:

- **.NET 8 SDK**
  - Download: [Microsoft .NET Downloads](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Visual Studio 2022**
  - Download: [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)
- **Web Browser**
  - Latest version of Chrome, Firefox, Safari, or any modern browser

---

## Getting Started

### Unzip the Application

1. **Locate the ZIP File:**
   - Find the `Number-to-Words.zip` file provided to you.

2. **Extract the ZIP File:**
   - Right-click the ZIP file and select **Extract All...** or use your preferred extraction tool.
   - Choose a destination folder where you want the application files to be extracted.

---

## Building and Running

### Using Visual Studio 2022

1. **Open the Solution:**
   - Navigate to the extracted folder.
   - Double-click the solution file `Number-to-Words.sln` to open the project in Visual Studio 2022.

2. **Restore NuGet Packages:**
   - Visual Studio will automatically restore NuGet packages upon opening the solution.
   - If not, right-click on the solution in **Solution Explorer** and select **Restore NuGet Packages**.

3. **Set Startup Project:**
   - Ensure the main project is set as the startup project.
   - Right-click on the `Number-to-Words.web` project in **Solution Explorer** and select **Set as Startup Project**.

4. **Run the Application:**
   - Press `F5` to run with debugging or `Ctrl+F5` to run without debugging.

5. **Access the Application:**
   - Your default browser will open at one of the following URLs:
     - **HTTPS:** `https://localhost:7041`
     - **HTTP:** `http://localhost:5126`
   - If it doesn't open automatically, open your browser and navigate to one of the URLs specified in the `launchSettings.json` file.

   **Note:** The application is configured to run on both HTTP and HTTPS ports as defined in the `launchSettings.json`:

   ```json
   "profiles": {
     "http": {
       "applicationUrl": "http://localhost:5126"
     },
     "https": {
       "applicationUrl": "https://localhost:7041;http://localhost:5126"
     }
   }
   ```

---

## Usage

1. **Enter a Number:**
   - On the home page, input a number into the field provided.   

2. **Convert:**
   - Click the **Convert** button or press **Enter**.

3. **View the Result:**
   - The converted amount in words will be displayed below the input field.

4. **Use the Keypad:**
   - You can use the on-screen keypad to input numbers.

5. **Conversion History:**
   - Previous conversion history is displayed below the result for future reference.

---

## Project Structure

- **Controllers**
  - `HomeController.cs`: Handles web requests and responses.

- **Models**
  - `NumberToWordsViewModel.cs`: Defines the data model for the conversion.

- **Services**
  - `INumberToWordsService.cs`: Interface for the conversion service.
  - `NumberToWordsService.cs`: Implements the conversion logic.
  - `ICurrencySettings.cs`: Interface for currency settings.
  - `DollarCurrencySettings.cs`: Implements settings for dollars and cents.
  - `ILanguageConfiguration.cs`: Interface for language configurations.
  - `EnglishLanguageConfiguration.cs`: Implements English language specifics.
  - `INumberToWordsConverter.cs`: Interface for number-to-words conversion.
  - `EnglishNumberToWordsConverter.cs`: Converts numbers to English words.

- **Views**
  - `Index.cshtml`: The main page view.

- **wwwroot**
  - Contains static files including CSS and JavaScript.

---

## Dependency Injection

The application utilises Dependency Injection to manage service lifetimes and dependencies, promoting loose coupling.

### Service Registration

In `Program.cs`, services are registered with the DI container:

```csharp
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            // Currency settings and number to words service
            builder.Services.AddSingleton<ICurrencySettings, DollarCurrencySettings>();

            // Language configurations and converters
            builder.Services.AddScoped<ILanguageConfiguration, EnglishLanguageConfiguration>();
            builder.Services.AddScoped<INumberToWordsConverter, EnglishNumberToWordsConverter>();

            builder.Services.AddTransient<INumberToWordsService, NumberToWordsService>();

            var app = builder.Build();
...
```

### Benefits of Dependency Injection

- **Modularity:** Easily swap out implementations (e.g., for different languages or currencies).
- **Testability:** Simplifies unit testing by allowing mock implementations.
- **Maintainability:** Centralises service configurations and dependencies.

---

## Customisation

### Supporting Other Currencies

1. **Create a Currency Settings Class:**

   - In the **Services** folder, create a new class (e.g., `EuroCurrencySettings.cs`).
   - Implement `ICurrencySettings`.
   - Define `CurrencyName`, `SubunitName`, and `DecimalPlaces`.

     ```csharp
     public class EuroCurrencySettings : ICurrencySettings
     {
         public string CurrencyName { get; } = "EURO";
         public string SubunitName { get; } = "CENT";
         public int DecimalPlaces { get; } = 2;
     }
     ```

2. **Register the New Currency Settings:**

   - In `Program.cs`, replace the existing currency settings registration:

     ```csharp
     builder.Services.AddSingleton<ICurrencySettings, EuroCurrencySettings>();
     ```

### Supporting Other Languages

1. **Create Language Configuration and Converter:**

   - In the **Services** folder, create a new class (e.g., `SpanishLanguageConfiguration.cs`) implementing `ILanguageConfiguration`.
   - In the **Services** folder, create a new class (e.g., `SpanishNumberToWordsConverter.cs`) implementing `INumberToWordsConverter`.
   - Define language specific word mappings and rules.

2. **Register the New Language Services:**

   - In `Program.cs`, register the new services:

     ```csharp
     builder.Services.AddScoped<ILanguageConfiguration, SpanishLanguageConfiguration>();
     builder.Services.AddScoped<INumberToWordsConverter, SpanishNumberToWordsConverter>();
     ```

---


