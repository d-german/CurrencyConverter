namespace CurrencyConverter.Dto;

public class ExchangeRateResponse
{
    public DateTime Date { get; set; }
    public Info Info { get; set; }
    public Query Query { get; set; }
    public double Result { get; set; }
    public bool Success { get; set; }
}
