namespace ExchangeQuotes.Server.Abstractions
{
    internal interface IExchangeQuotesSender //: IDisposable
    {
        void SendData(double exchangeQuotes);
    }
}