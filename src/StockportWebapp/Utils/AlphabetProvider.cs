namespace StockportWebapp.Utils;

public static class AlphabetProvider
{
    public static IEnumerable<char> GetAlphabet()
    {
        for (char letter = 'A'; letter <= 'Z'; letter++)
        {
            yield return letter;
        }
    }
}