namespace Number_to_Words.web.Services
{
    public interface ICurrencySettings
    {
        string CurrencyName { get; }
        string SubunitName { get; }
        int DecimalPlaces { get; }
    }
}