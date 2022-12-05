using ExchangeQuotes.Math.Abstractions;

namespace ExchangeQuotes.Math.Statistic;

public class StandardDeviationCalculator : IStatisticThreadSafeCalculator
{
    private int _count;
    private double _mean;
    private double _dSquared;

    public void AddNumberToSequence(int number)
    {
        _count++;

        var meanDifferential = (number - _mean) / _count;

        var newMean = _mean + meanDifferential;

        var dSquaredIncrement = (number - newMean) * (number - _mean);

        var newDSquared = _dSquared + dSquaredIncrement;

        _mean = newMean;

        _dSquared = newDSquared;
    }

    public double GetCurrentResult() => System.Math.Sqrt(SampleVariance());

    public double SampleVariance() => _count > 1 ? _dSquared / (_count - 1) : 0;
}