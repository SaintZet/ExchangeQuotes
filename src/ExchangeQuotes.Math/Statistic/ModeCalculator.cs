using ExchangeQuotes.Math.Abstractions;
using System.Collections.Concurrent;

namespace ExchangeQuotes.Math.Statistic;

public class ModeCalculator : IStatisticThreadSafeCalculator
{
    private readonly ConcurrentDictionary<double, long> _sequence = new();

    public void AddNumberToSequence(int number)
    {
        if (!_sequence.TryAdd(number, 1))
        {
            _sequence[number] = _sequence[number] + 1;
        }
    }

    public double GetCurrentResult()
    {
        if (_sequence.IsEmpty)
        {
            return 0;
        }

        return _sequence.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
    }
}