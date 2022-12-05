using ExchangeQuotes.Client.Abstractions;
using ExchangeQuotes.Client.Models;
using ExchangeQuotes.Math.Abstractions;
using ExchangeQuotes.Math.Statistic;

namespace ExchangeQuotes.Client.Services;

internal class ExchangeQuotesCalculateStatistic : IExchangeQuotesCalculateWorker
{
    private readonly IStatisticThreadSafeCalculator _averageCalculator, _standardDeviationCalculator, _modeCalculator, _medianCalculator;
    private readonly List<IStatisticThreadSafeCalculator> _statisticCalculators;

    public ExchangeQuotesCalculateStatistic()
    {
        _averageCalculator = new AverageCalculator();
        _standardDeviationCalculator = new StandardDeviationCalculator();
        _modeCalculator = new ModeCalculator();
        _medianCalculator = new MedianCalculator();

        _statisticCalculators = new List<IStatisticThreadSafeCalculator>
        {
            _averageCalculator,
            _standardDeviationCalculator,
            _modeCalculator,
            _medianCalculator,
        };
    }

    public ExchangeQuotesStatistic GetCurrentValues() => new()
    {
        Average = _averageCalculator.GetCurrentResult(),
        Median = _medianCalculator.GetCurrentResult(),
        Mode = _modeCalculator.GetCurrentResult(),
        StandardDeviation = _standardDeviationCalculator.GetCurrentResult(),
    };

    public void CalculateValues(byte[] bytes)
    {
        var exchangeQuote = BitConverter.ToInt32(bytes!, 0);

        _statisticCalculators.ForEach(c => c.AddNumberToSequence(exchangeQuote));
    }
}