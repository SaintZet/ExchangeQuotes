using ExchangeQuotes.Math.Abstractions;

namespace ExchangeQuotes.Math.Statistic
{
    public class AverageCalculator : IStatisticCalculator
    {
        private double _currentResult;
        private int _count;

        public void AddNumberToSequence(double number)
        {
            double k = _count + 1;
            _currentResult = ((_count / k) * _currentResult) + (number / k);

            _count++;
        }

        public double GetCurrentResult() => _currentResult;
    }
}