using ExchangeQuotes.Math.Abstractions;

namespace ExchangeQuotes.Math.Statistic;

public class StandardDeviationCalculator : IStatisticThreadSafeCalculator
{
    private readonly CalculatedData _calculatedData = new();

    public void AddNumberToSequence(int number)
    {
        lock (_calculatedData)
        {
            _calculatedData.Count++;

            var meanDifferential = (number - _calculatedData.Mean) / _calculatedData.Count;

            var newMean = _calculatedData.Mean + meanDifferential;

            var dSquaredIncrement = (number - newMean) * (number - _calculatedData.Mean);

            var newDSquared = _calculatedData.DeviationSquared + dSquaredIncrement;

            _calculatedData.Mean = newMean;

            _calculatedData.DeviationSquared = newDSquared;
        }
    }

    public double GetCurrentResult()
    {
        lock (_calculatedData)
        {
            var sampleVariance = _calculatedData.Count > 1 ? _calculatedData.DeviationSquared / (_calculatedData.Count - 1) : 0;

            return System.Math.Sqrt(sampleVariance);
        }
    }

    private class CalculatedData
    {
        public int Count { get; set; }
        public double Mean { get; set; }
        public double DeviationSquared { get; set; }
    }
}