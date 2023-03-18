namespace CurrencyConverter.Models;

public class CurrencyConversionViewModel
{
    public string SourceCurrency { get; set; }
    public string TargetCurrency { get; set; }
    public double InputAmount { get; set; }
    public double? ConvertedAmount { get; set; }
}
