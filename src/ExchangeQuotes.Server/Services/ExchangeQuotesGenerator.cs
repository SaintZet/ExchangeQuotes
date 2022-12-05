using ExchangeQuotes.Server.Abstractions;

namespace ExchangeQuotes.Server.Services;

internal class RandomExchangeQuotesGenerator : IExchangeQuotesProvider
{
    private readonly Random _random = new();

    private readonly double _minValue;
    private readonly double _maxValue;

    public RandomExchangeQuotesGenerator(double minValue, double maxValue)
    {
        _minValue = minValue;
        _maxValue = maxValue;
    }

    public double CurrentExchangeQuote()
    {
        var random = _minValue + (_random.NextDouble() * (_maxValue - _minValue));

        return Math.Round(random, 4);
    }
}