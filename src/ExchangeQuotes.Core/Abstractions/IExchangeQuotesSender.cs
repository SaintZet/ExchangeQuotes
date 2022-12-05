namespace ExchangeQuotes.Core.Abstractions;

public interface IExchangeQuotesSender
{
    void SendData(double exchangeQuotes);
}