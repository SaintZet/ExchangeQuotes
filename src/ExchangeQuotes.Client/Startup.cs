using ExchangeQuotes.Client.Abstractions;
using ExchangeQuotes.Client.Models;
using ExchangeQuotes.Client.Services;
using ExchangeQuotes.Core.Abstractions;
using ExchangeQuotes.Core.Communication;
using ExchangeQuotes.Core.Сonfiguration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeQuotes.Client;

internal class Startup
{
    public Startup()
    {
        Configuration = LoadConfiguration(new XmlConfigProvider<Config>("ClientConfig.xml"));
    }

    public Config Configuration { get; }

    public IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection()

        .AddSingleton<IExchangeQuotesCalculateWorker, ExchangeQuotesCalculateStatistic>()
        .AddSingleton<IExchangeQuotesView, ExchangeQuotesConsole>()
        .AddSingleton<IExchangeQuotesReceiver>(new UdpClientWrapper(Configuration.Port, Configuration.MulticastIPAddress))
        .AddSingleton(s => new Application(
            s.GetRequiredService<IExchangeQuotesReceiver>(),
            s.GetRequiredService<IExchangeQuotesCalculateWorker>(),
            s.GetRequiredService<IExchangeQuotesView>())
        );

        return services.BuildServiceProvider();
    }

    private Config LoadConfiguration(IConfigProvider<Config> configProvider)
    {
        return configProvider.GetOrCreateDefaultConfig();
    }
}