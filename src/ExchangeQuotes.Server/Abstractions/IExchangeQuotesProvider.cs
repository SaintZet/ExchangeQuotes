namespace ExchangeQuotes.Server.Abstractions;

internal interface IExchangeQuotesProvider
{
    double CurrentExchangeQuote();
}