namespace ExchangeQuotes.Core.Abstractions
{
    public interface IConfigProvider<T> where T : new()
    {
        public T GetOrCreateDefaultConfig();

        public void CreateDefaultConfig();
    }
}