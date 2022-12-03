using ExchangeQuotes.Client.Abstractions;
using ExchangeQuotes.Client.Models;
using ExchangeQuotes.Math.Abstractions;
using ExchangeQuotes.Math.Statistic;
using System.Collections.Concurrent;

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

        //TODO: maybe it's not thrad safe?
        public ExchangeQuotesStatistic CurrentValues => new()
        {
            Average = _averageCalculator.GetCurrentResult(),
            Median = _medianCalculator.GetCurrentResult(),
            Mode = _modeCalculator.GetCurrentResult(),
            StandardDeviation = _standardDeviationCalculator.GetCurrentResult(),
        };

        //TODO: Maybe add cancellation token
        public void DoWork(ref ConcurrentQueue<double> exchangeQuotes, ref AutoResetEvent signal)
        {
            while (true)
            {
                signal.WaitOne();

                while (exchangeQuotes.TryDequeue(out double exchangeQuote))
                {
                    foreach (var calculator in _statisticCalculators)
                    {
                        calculator.AddNumberToSequence(exchangeQuote);
                    }
                }
            }
        }
    }
}