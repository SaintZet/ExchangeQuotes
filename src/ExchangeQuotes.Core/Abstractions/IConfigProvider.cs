namespace ExchangeQuotes.Core.Abstractions;

public interface IConfigProvider<T> where T : new()
{
    T GetConfigOrCreateDefault();

    void CreateDefaultConfig();
}