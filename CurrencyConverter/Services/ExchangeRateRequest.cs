namespace CurrencyConverter.Services;

public class ExchangeRateRequest
{
    public string TargetCurrency { get; set; }
    public string SourceCurrency { get; set; }
    public double InputAmount { get; set; }
}