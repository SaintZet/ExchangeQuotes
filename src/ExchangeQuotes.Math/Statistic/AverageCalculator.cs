using ExchangeQuotes.Math.Abstractions;

namespace ExchangeQuotes.Math.Statistic;

public class AverageCalculator : IStatisticThreadSafeCalculator
{
    private volatile object _currentResult;
    private volatile object _count;

    public AverageCalculator()
    {
        _currentResult = 0.0;
        _count = 0;
    }

    public void AddNumberToSequence(int number)
    {
        int currentCount = (int)_count;
        int newCount = currentCount + 1;

        double currentResult = (double)_currentResult;
        double x = ((double)number / (double)newCount);
        double y = ((double)currentCount / (double)newCount);
        double newResult = (y * currentResult) + x;

        _currentResult = (object)newResult;
        _count = (object)newCount;
    }

    public double GetCurrentResult()
    {
        return (double)_currentResult;
    }
}