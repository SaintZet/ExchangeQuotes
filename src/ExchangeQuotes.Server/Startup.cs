using ExchangeQuotes.Core.Abstractions;
using ExchangeQuotes.Core.Communication.Udp;
using ExchangeQuotes.Core.Сonfiguration;
using ExchangeQuotes.Server.Abstractions;
using ExchangeQuotes.Server.Constants;
using ExchangeQuotes.Server.Models;
using ExchangeQuotes.Server.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeQuotes.Server;

internal class Startup
{
    public Startup()
    {
        Configuration = LoadConfiguration(new XmlConfigProvider<Config>(StartupConstants.PathToConfigFile));
    }

    public Config Configuration { get; }

    public IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection()

        .AddSingleton<IExchangeQuotesProvider>(new RandomExchangeQuotesGenerator(Configuration.MinValue, Configuration.MaxValue))
        .AddSingleton<IExchangeQuotesSender>(new UdpClientWrapper(Configuration.Port, Configuration.MulticastIPAddress))
        .AddSingleton(s => new Application(s.GetRequiredService<IExchangeQuotesSender>(), s.GetRequiredService<IExchangeQuotesProvider>()))
        ;

        return services.BuildServiceProvider();
    }

    private Config LoadConfiguration(IConfigProvider<Config> configProvider)
    {
        return configProvider.GetConfigOrCreateDefault();
    }
}