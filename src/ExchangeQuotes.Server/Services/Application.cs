using ExchangeQuotes.Core.Abstractions;
using ExchangeQuotes.Server.Abstractions;

namespace ExchangeQuotes.Server.Services;

internal class Application
{
    private readonly IExchangeQuotesSender _exchangeQuotesSender;
    private readonly IExchangeQuotesProvider _exchangeQuotesProvider;

    public Application(IExchangeQuotesSender exchangeQuotesSender, IExchangeQuotesProvider exchangeQuotesProvider)
    {
        _exchangeQuotesSender = exchangeQuotesSender;
        _exchangeQuotesProvider = exchangeQuotesProvider;
    }

    internal void StartDoWork()
    {
        int i = 0;
        while (true)
        {
            int data = _exchangeQuotesProvider.CurrentExchangeQuote();

            _exchangeQuotesSender.SendData(data);

#if DEBUG
            Console.WriteLine(data.ToString());
#endif
        }
    }
}