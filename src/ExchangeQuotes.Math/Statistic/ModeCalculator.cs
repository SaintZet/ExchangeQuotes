using ExchangeQuotes.Math.Abstractions;

namespace ExchangeQuotes.Math.Statistic;

public class ModeCalculator : IStatisticCalculator
{
    private readonly Dictionary<double, int> _sequence = new();

    public void AddNumberToSequence(double number)
    {
        if (_sequence.ContainsKey(number))
        {
            _sequence[number] = _sequence[number] + 1;
        }
        else
        {
            _sequence.Add(number, 1);
        }
    }

    public double GetCurrentResult()
    {
        if (_sequence.Count == 0)
        {
            return 0;
        }

        Dictionary<double, int> copy = new(_sequence);

        return copy.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
    }
}