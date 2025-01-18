# **Number-to-Words Design Decisions and Philosophy**

## **Introduction**

The decisions made while implementing the application design and algorithm were based on extensibility to allow for future modifcations to languages and currencies.

---

## **Algorithm Overview**

The conversion processing of numbers to words has been modularised by focussing on separation of concern for the various tasks; specifically separating the language specific logic from the core conversion logic. 

### **Key Components**

1. **`NumberToWordsService`**: The main service that handles the overall conversion process.
2. **`ICurrencySettings`**: Interface for currency specific settings including currency name, subunit name, and decimal places.
3. **`ILanguageConfiguration`**: Interface for language specific configurations including word mappings.
4. **`INumberToWordsConverter`**: Interface defining the method for converting integers to words in a specific language.
5. **Language Specific Implementations**: Classes that implement `ILanguageConfiguration` and `INumberToWordsConverter` for each language (currently English only).

---

## **Detailed Algorithm Explanation**

### **1. NumberToWordsService**

The `NumberToWordsService` class facilitates the conversion of a decimal variable into words.

```csharp
public class NumberToWordsService : INumberToWordsService
{
    private readonly ICurrencySettings _currencySettings;
    private readonly INumberToWordsConverter _numberToWordsConverter;
    private readonly ILanguageConfiguration _languageConfig;

    public NumberToWordsService(ICurrencySettings currencySettings, INumberToWordsConverter numberToWordsConverter, ILanguageConfiguration languageConfig)
    {
        _currencySettings = currencySettings;
        _numberToWordsConverter = numberToWordsConverter;
        _languageConfig = languageConfig;
    }

    public string ConvertNumberToWords(decimal number)
    {
        string words = "";

        // Get the dollar and cents parts
        long multiplier = (long)Math.Pow(10, _currencySettings.DecimalPlaces);
        long units = (long)(Math.Abs(number) * multiplier);
        long dollars = units / multiplier;
        long cents = units % multiplier;

        bool hasCents = _currencySettings.DecimalPlaces > 0;

        // Convert dollars part
        words += $"{_numberToWordsConverter.ConvertIntegerToWords(dollars)} {_currencySettings.CurrencyName.ToUpper()}{(dollars != 1 ? "S" : "")}";

        // Convert cents part
        if (hasCents)
        {
            words += $" {_languageConfig.AndWord} ";
            words += $"{_numberToWordsConverter.ConvertIntegerToWords(cents)} {_currencySettings.SubunitName.ToUpper()}{(cents != 1 ? "S" : "")}";
        }

        return words.Trim();
    }

}
```

**Explanation**

- **Currency Separation:**
  - The input decimal variable is split into currency units and subunits based on the number of decimal places defined in `_currencySettings.DecimalPlaces`.
  
- **Conversion Process:**
  - The process starts with intitialising an empty string 'words'
  - Since the code has been designed so that decimal places can be increased or decreased, a multiplier is calculated. (e.g. if the decimal places are set to 2, then the multiplier is set to 100 (10^2))
  - The multiplier is used to separate the dollars and cents parts; first by multipying the number by the multiplier and then obtaining the dollars and cents by using divide and mod with the multiplier respectively to obtain the separate currency units. 
  - If the currency units (i.e., dollars or cents) are greater or equal to zero, they are converted to words using `_numberToWordsConverter.ConvertIntegerToWords`.
  - The currency name is then appended with handling of pluralisation based on the unit count.
  - The language-specific word for "and" is used from `_languageConfig.AndWord`.


### **2. EnglishNumberToWordsConverter**

The `EnglishNumberToWordsConverter` class implements `INumberToWordsConverter` and contains the logic to convert integer numbers into English words.

```csharp
public class EnglishNumberToWordsConverter : INumberToWordsConverter
{
    private readonly ILanguageConfiguration _config;

    public EnglishNumberToWordsConverter(ILanguageConfiguration config)
    {
        _config = config;
    }

    public string ConvertIntegerToWords(long number)
    {

        string words = "";

        foreach (var scale in _config.ScaleMap.Keys)
        {
            if (number / scale > 0)
            {
                words += $"{ConvertIntegerToWords(number / scale)} {_config.ScaleMap[scale]} ";
                number %= scale;
            }
        }

        if (number >= 0)
        {
            if (!string.IsNullOrEmpty(words))
                words += $"{_config.AndWord} ";

            if (number < 20)
                words += _config.UnitsMap[(int)number];
            else
            {
                words += _config.TensMap[((int)(number / 10)) * 10];
                if ((number % 10) > 0)
                    words += $"-{_config.UnitsMap[(int)(number % 10)]}";
            }
        }

        return words.Trim();
    }
}
```

**Explanation:**

- Using recursion, the ScaleMap is iterated through to process large numbers (e.g., billions, millions, thousands, hundreds).
- For numbers greater than or equal to 20, the TensMap is used and for numbers less than 20 the UnitsMap is used. 


### **3. ILanguageConfiguration**

The `ILanguageConfiguration` interface defines language-specific properties required for conversion.

**Properties:**

- `UnitsMap`: A dictionary mapping units (0-19) to their word representations.
- `TensMap`: A dictionary mapping tens (20, 30, ..., 90) to words.
- `ScaleMap`: A dictionary mapping scale numbers (e.g., 1,000,000) to words (e.g., "MILLION").
- `AndWord`: The word used for "and" in the language.

```csharp
public class EnglishLanguageConfiguration : ILanguageConfiguration
{
    public IDictionary<int, string> UnitsMap => new Dictionary<int, string>
    {
        [0] = "ZERO",
        [1] = "ONE",
        [2] = "TWO",
        [3] = "THREE",
        [4] = "FOUR",
        [5] = "FIVE",
        [6] = "SIX",
        [7] = "SEVEN",
        [8] = "EIGHT",
        [9] = "NINE",
        [10] = "TEN",
        [11] = "ELEVEN",
        [12] = "TWELVE",
        [13] = "THIRTEEN",
        [14] = "FOURTEEN",
        [15] = "FIFTEEN",
        [16] = "SIXTEEN",
        [17] = "SEVENTEEN",
        [18] = "EIGHTEEN",
        [19] = "NINETEEN"
    };

    public IDictionary<int, string> TensMap => new Dictionary<int, string>
    {
        [20] = "TWENTY",
        [30] = "THIRTY",
        [40] = "FORTY",
        [50] = "FIFTY",
        [60] = "SIXTY",
        [70] = "SEVENTY",
        [80] = "EIGHTY",
        [90] = "NINETY"
    };

    public IDictionary<long, string> ScaleMap => new Dictionary<long, string>
    {
        [1000000000] = "BILLION",
        [1000000] = "MILLION",
        [1000] = "THOUSAND",
        [100] = "HUNDRED"
    };

    public string AndWord => "AND";

}
```

---

## **Why This Algorithm and Design Were Chosen**

As previously mentioned, extensibility was my main focus when designing this application. The language specific logic abstraction allows for new languages to be added without having to modify existing code. The modular design provides a clean and maintainable code base; responsibilities are separated into classes and interfaces, adhering to the Single Responsibility Principle. The application is open for extension (e.g. new languages) but closed off for modification of existing code. High-level modules (`NumberToWordsService`) depend on abstractions (`INumberToWordsConverter`), not concrete implementations. Overall, my approach successfully adheres to the SOLID principles of object oriented design. 

## **Benefits of the Chosen Approach**

- **Scalability:**
  - New languages and currencies can be added with minimal changes to existing code.

- **Customisability:**
  - Language specific grammatical rules and exceptions can be handled within their respective configurations.

- **Testing and Reliability:**
  - Isolated components make it easier to write unit tests and ensure reliability.

- **Future-Proofing:**
  - The design anticipates future requirements, reducing technical debt and the need for major refactoring.

---

## **Alternative Approaches and Reasons for Not Choosing Them**

### **1. Hardcoding Language Logic in the Core Service**

- **Lack of Extensibility:**
  - Adding new languages would require modifying the core service which would increase the risk of introducing bugs.
  
- **Maintainability Issues:**
  - Mixing language logic with core functionality leads to code that is harder to read and maintain.

### **2. Using Conditional Statements for Language Selection**

- **Scalability Concerns:**
  - Each new language adds complexity, making future maintenance increasingly difficult.
