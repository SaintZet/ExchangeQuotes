using UdpMulticast.Server.Abstractions;

namespace UdpMulticast.Server.Services
{
    internal class RandomExchangeQuotesGenerator<T> : IExchangeQuotesProvider<T> where T : struct
    {
        private readonly Random _random = new();

        private readonly T _minValue;
        private readonly T _maxValue;

        public RandomExchangeQuotesGenerator(T minValue, T maxValue)
        {
            _minValue = minValue!;
            _maxValue = maxValue!;
        }

        public T CurrentExchangeQuote()
        {
            //TODO: delete dynamic
            return _minValue + (_random.NextDouble() * ((dynamic)_maxValue - _minValue));
        }
    }
}