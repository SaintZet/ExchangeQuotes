using ExchangeQuotes.Core.Abstractions;
using ExchangeQuotes.Core.Communication;
using ExchangeQuotes.Core.Сonfiguration;
using ExchangeQuotes.Server.Abstractions;
using ExchangeQuotes.Server.Models;
using ExchangeQuotes.Server.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeQuotes.Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var config = LoadConfiguration(new XmlConfigProvider<Config>("ServerConfig.xml"));
            var provider = ConfigureServices(config);
            var app = provider.GetRequiredService<Application>();

            app.StartDoWork(true);
        }

        private static Config LoadConfiguration(IConfigProvider<Config> configProvider)
        {
            return configProvider.GetOrCreateDefaultConfig();
        }

        private static IServiceProvider ConfigureServices(Config config)
        {
            var services = new ServiceCollection()
            .AddSingleton<IExchangeQuotesProvider>(new RandomExchangeQuotesGenerator(config.MinValue, config.MaxValue))
            .AddSingleton<IExchangeQuotesSender>(new UdpClientWrapper(config.Port, config.MulticastIPAddress))
            .AddSingleton(s => new Application(s.GetRequiredService<IExchangeQuotesSender>(), s.GetRequiredService<IExchangeQuotesProvider>()))
            ;

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    }
}