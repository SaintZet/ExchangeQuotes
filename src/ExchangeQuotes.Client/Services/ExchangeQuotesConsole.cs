using ExchangeQuotes.Client.Abstractions;
using ExchangeQuotes.Client.Models;
using System.Text;

namespace ExchangeQuotes.Client.Services;

internal class ExchangeQuotesConsole : IExchangeQuotesView
{
    public event EventHandler? RequestedData;

    public void DisplayData(ExchangeQuotesStatistic data, long packetLoss)
    {
        var stringBuilder = new StringBuilder()
                .AppendLine($"Average: {data.Average}")
                .AppendLine($"Median: {data.Median}")
                .AppendLine($"Mode: {data.Mode}")
                .AppendLine($"StandardDeviation: {data.StandardDeviation}")
                .AppendLine()
                .AppendLine($"Packet Loss: {packetLoss}");

        Console.WriteLine(stringBuilder.ToString());
    }

    public void StartDoWork()
    {
        while (true)
        {
            if (Console.ReadKey().Key == ConsoleKey.Enter)
            {
                RequestedData?.Invoke(this, new EventArgs());
            }
        }
    }
}