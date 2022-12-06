using Microsoft.Extensions.DependencyInjection;

namespace ExchangeQuotes.Client;

internal class Program
{
    private static void Main(string[] args)
    {
        Startup startup = new();
        IServiceProvider serviceProvider = startup.ConfigureServices();

        var app = serviceProvider.GetRequiredService<Application>();
        app.StartDoWork(startup.Configuration.TaskDelay);
    }
}