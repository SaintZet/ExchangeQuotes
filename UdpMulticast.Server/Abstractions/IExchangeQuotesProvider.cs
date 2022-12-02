namespace UdpMulticast.Server.Abstractions
{
    internal interface IExchangeQuotesProvider<T> where T : struct
    {
        T CurrentExchangeQuote();
    }
}