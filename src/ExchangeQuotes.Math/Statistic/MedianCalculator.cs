using ExchangeQuotes.Math.Abstractions;

namespace ExchangeQuotes.Math.Statistic
{
    public class MedianCalculator : IStatisticCalculator
    {
        private List<double> _sequence = new();

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
            int len = _sequence.Count();

            double mid = _sequence[len / 2];

            if (len % 2 == 1)
            {
                return mid;
            }
            else
            {
                return 1.0 * (_sequence[len / 2 - 1] + mid) / 2;
            }
        }
    }
}