namespace ExchangeQuotes.Server.Abstractions;

internal interface IExchangeQuotesProvider
{
    int CurrentExchangeQuote();
}