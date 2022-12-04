﻿using ExchangeQuotes.Client.Abstractions;
using ExchangeQuotes.Client.Models;
using System.Text;

namespace ExchangeQuotes.Client.Services
{
    internal class ExchangeQuotesConsole : IExchangeQuotesView
    {
        public event EventHandler? RequestedData;

        public void DisplayData(ExchangeQuotesStatistic data, int packetLoss)
        {
            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine($"Average: {data.Average}");
            stringBuilder.AppendLine($"Median: {data.Median}");
            stringBuilder.AppendLine($"Mode: {data.Mode}");
            stringBuilder.AppendLine($"StandardDeviation: {data.StandardDeviation}");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine($"Packet Loss: {packetLoss}");

            Console.WriteLine(stringBuilder.ToString());
        }

        public void StartWork()
        {
            while (true)
            {
                var key = Console.ReadKey();

                if (key.Key == ConsoleKey.Enter)
                {
                    RequestedData?.Invoke(this, new EventArgs());
                }
            }
        }
    }
}