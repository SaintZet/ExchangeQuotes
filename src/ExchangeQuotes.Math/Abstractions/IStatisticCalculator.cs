namespace ExchangeQuotes.Math.Abstractions;

public interface IStatisticCalculator
{
    void AddNumberToSequence(double number);

    double GetCurrentResult();
}