using ExchangeQuotes.Client.Models;

namespace ExchangeQuotes.Client.Abstractions;

internal interface IExchangeQuotesView
{
    event EventHandler? RequesteData;

    void StartDoWork();

    void DisplayData(ExchangeQuotesStatistic data, long packetLoss);
}