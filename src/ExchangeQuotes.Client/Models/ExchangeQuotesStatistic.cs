namespace ExchangeQuotes.Client.Models;

internal struct ExchangeQuotesStatistic
{
    public double Average { get; set; }
    public double StandardDeviation { get; set; }
    public double Mode { get; set; }
    public double Median { get; set; }
}