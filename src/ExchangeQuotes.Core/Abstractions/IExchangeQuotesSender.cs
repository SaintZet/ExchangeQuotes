namespace ExchangeQuotes.Core.Abstractions;

public interface IExchangeQuotesSender
{
    void SendData(int exchangeQuotes);
}