using ExchangeQuotes.Core.Abstractions;
using System.Xml;
using System.Xml.Serialization;

namespace ExchangeQuotes.Core.Сonfiguration;

public class XmlConfigProvider<T> : IConfigProvider<T> where T : new()
{
    private readonly string _pathToConfig;

    public XmlConfigProvider(string pathToConfig)
    {
        _pathToConfig = pathToConfig;
    }

    public T GetConfigOrCreateDefault()
    {
        if (!File.Exists(_pathToConfig))
        {
            CreateDefaultConfig();
        }

        XmlSerializer serializer = new(typeof(T));

        using StreamReader reader = new(_pathToConfig);

        T config = (T)serializer.Deserialize(reader)!;

        reader.Close();

        return config;
    }

    public void CreateDefaultConfig()
    {
        T config = new();

        var xmlWriterSettings = new XmlWriterSettings() { Indent = true };

        XmlSerializer serializer = new(typeof(T));

        using XmlWriter xmlWriter = XmlWriter.Create(_pathToConfig, xmlWriterSettings);

        serializer.Serialize(xmlWriter, config);
    }
}