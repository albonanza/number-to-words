namespace Number_to_Words.web.Services
{
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
}