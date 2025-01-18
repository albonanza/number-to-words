namespace Number_to_Words.web.Services
{
    public class DollarCurrencySettings : ICurrencySettings
    {
        public string CurrencyName { get; } = "Dollar";
        public string SubunitName { get; } = "Cent";
        public int DecimalPlaces { get;  } = 2;
    }
}