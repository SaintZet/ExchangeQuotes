using ExchangeQuotes.Math.Abstractions;

namespace ExchangeQuotes.Math.Statistic;

public class AverageCalculator : IStatisticThreadSafeCalculator
{
    private readonly CalculatedData _calculatedData = new();

    public void AddNumberToSequence(int number)
    {
        lock (_calculatedData)
        {
            double k = _calculatedData.Count + 1;
            _calculatedData.CurrentResult = ((_calculatedData.Count / k) * _calculatedData.CurrentResult) + (number / k);

            _calculatedData.Count++;
        }
    }

    public double GetCurrentResult()
    {
        lock (_calculatedData)
        {
            return _calculatedData.CurrentResult;
        }
    }

    private class CalculatedData
    {
        public int Count { get; set; }
        public double CurrentResult { get; set; }
    }
}