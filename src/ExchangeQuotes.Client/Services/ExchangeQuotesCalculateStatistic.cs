using ExchangeQuotes.Client.Abstractions;
using ExchangeQuotes.Client.Models;
using ExchangeQuotes.Math.Abstractions;
using ExchangeQuotes.Math.Statistic;

namespace ExchangeQuotes.Client.Services
{
    internal class ExchangeQuotesCalculateStatistic : IExchangeQuotesCalculateWorker
    {
        private readonly IStatisticCalculator _averageCalculator, _standardDeviationCalculator, _modeCalculator, _medianCalculator;
        private readonly IList<IStatisticCalculator> _statisticCalculators;

        public ExchangeQuotesCalculateStatistic()
        {
            _averageCalculator = new AverageCalculator();
            _standardDeviationCalculator = new StandardDeviationCalculator();
            _modeCalculator = new ModeCalculator();
            _medianCalculator = new MedianCalculator();

            _statisticCalculators = new List<IStatisticCalculator>
            {
                _averageCalculator,
                _standardDeviationCalculator,
                _modeCalculator,
                _medianCalculator,
            };
        }

        public ExchangeQuotesStatistic GetCurrentValues()
        {
            return new()
            {
                Average = _averageCalculator.GetCurrentResult(),
                Median = _medianCalculator.GetCurrentResult(),
                Mode = _modeCalculator.GetCurrentResult(),
                StandardDeviation = _standardDeviationCalculator.GetCurrentResult(),
            };
        }

        public void CalculateValues(byte[] bytes)
        {
            var exchangeQuote = BitConverter.ToDouble(bytes!, 0);

            foreach (var calculator in _statisticCalculators)
            {
                calculator.AddNumberToSequence(exchangeQuote);
            }
        }
    }
}