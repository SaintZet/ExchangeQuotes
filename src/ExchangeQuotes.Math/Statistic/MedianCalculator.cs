using ExchangeQuotes.Math.Abstractions;

namespace ExchangeQuotes.Math.Statistic;

public class MedianCalculator : IStatisticCalculator
{
    private List<double> _sequence = new(1000000000);

    public MedianCalculator()
    {
        _sequence.Add(0);
    }

    public void AddNumberToSequence(double number)
    {
        int index = _sequence.BinarySearch(number);

        if (index >= 0)
        {
            _sequence.Insert(index, number);
        }
        else
        {
            _sequence.Insert(-index - 1, number);
        }
    }

    public double GetCurrentResult()
    {
        if (_sequence.Count == 0)
        {
            return 0;
        }

        int len = _sequence.Count;

        double mid = _sequence[len / 2];

        return len % 2 == 1 ? mid : 1.0 * (_sequence[len / 2 - 1] + mid) / 2;
    }
}