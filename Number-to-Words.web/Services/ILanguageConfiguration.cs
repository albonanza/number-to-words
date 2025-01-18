namespace Number_to_Words.web.Services
{
    public interface ILanguageConfiguration
    {
        IDictionary<int, string> UnitsMap { get; }
        IDictionary<int, string> TensMap { get; }
        IDictionary<long, string> ScaleMap { get; }
        string AndWord { get; }

    }
}