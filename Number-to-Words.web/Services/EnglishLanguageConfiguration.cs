namespace Number_to_Words.web.Services
{
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
}