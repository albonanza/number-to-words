namespace Number_to_Words.web.Services
{
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
}