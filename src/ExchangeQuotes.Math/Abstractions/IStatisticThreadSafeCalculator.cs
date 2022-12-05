namespace ExchangeQuotes.Math.Abstractions;

public interface IStatisticThreadSafeCalculator
{
    void AddNumberToSequence(int number);

    double GetCurrentResult();
}