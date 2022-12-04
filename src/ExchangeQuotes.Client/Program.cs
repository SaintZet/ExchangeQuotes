using ExchangeQuotes.Client.Abstractions;
using ExchangeQuotes.Client.Models;
using ExchangeQuotes.Client.Services;
using ExchangeQuotes.Core.Abstractions;
using ExchangeQuotes.Core.Communication;
using ExchangeQuotes.Core.Сonfiguration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeQuotes.Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var config = LoadConfiguration(new XmlConfigProvider<Config>("ClientConfig.xml"));
            var provider = ConfigureServices(config);
            var app = provider.GetRequiredService<Application>();

            app.StartDoWork(config.TaskDelay);
        }

        private static Config LoadConfiguration(IConfigProvider<Config> configProvider)
        {
            return configProvider.GetOrCreateDefaultConfig();
        }

        private static IServiceProvider ConfigureServices(Config config)
        {
            var services = new ServiceCollection()

            .AddSingleton<IExchangeQuotesCalculateWorker, ExchangeQuotesCalculateStatistic>()
            .AddSingleton<IExchangeQuotesView, ExchangeQuotesConsole>()
            .AddSingleton<IExchangeQuotesReceiver>(new UdpClientWrapper(config.Port, config.MulticastIPAddress))
            .AddSingleton(s => new Application(s.GetRequiredService<IExchangeQuotesReceiver>(),
                                                s.GetRequiredService<IExchangeQuotesCalculateWorker>(),
                                                s.GetRequiredService<IExchangeQuotesView>()))
            ;

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    }
}