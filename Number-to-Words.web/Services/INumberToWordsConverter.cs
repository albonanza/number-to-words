namespace Number_to_Words.web.Services
{
    public interface INumberToWordsConverter
    {
        string ConvertIntegerToWords(long number);
    }
}