using ExchangeQuotes.Server.Abstractions;

namespace ExchangeQuotes.Server.Services;

internal class RandomExchangeQuotesGenerator : IExchangeQuotesProvider
{
    private readonly Random _random = new();

    private readonly int _minValue;
    private readonly int _maxValue;

    public RandomExchangeQuotesGenerator(int minValue, int maxValue)
    {
        _minValue = minValue;
        _maxValue = maxValue;
    }

    public int CurrentExchangeQuote()
    {
        return _random.Next(_minValue, _maxValue);
    }
}