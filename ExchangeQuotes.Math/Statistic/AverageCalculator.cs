using ExchangeQuotes.Math.Abstractions;

namespace ExchangeQuotes.Math.Statistic
{
    public class AverageCalculator : IStatisticCalculator
    {
        private double _sum;
        private int _count;

        public void AddNumberToSequence(double number)
        {
            _sum += number;
            _count++;
        }

        public double GetCurrentResult()
        {
            return _sum / _count;
        }
    }
}