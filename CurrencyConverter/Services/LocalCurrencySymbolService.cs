namespace CurrencyConverter.Services;

public class LocalCurrencySymbolService : ICurrencySymbolService
{
    public string[] GetCurrencySymbols()
    {
        // Extended list of in-memory currency symbols for local development
        return new string[] {
            "USD", "EUR", "GBP", "JPY", "AUD", 
            "CAD", "CHF", "CNY", "SEK", "NZD",
            "MXN", "SGD", "HKD", "NOK", "KRW",
            "TRY", "RUB", "INR", "BRL", "ZAR",
            "DKK", "PLN", "TWD", "THB", "MYR",
            "IDR", "HUF", "CZK", "ILS", "CLP",
            "PHP", "AED", "COP", "SAR", "ISK",
            "EGP", "NGN", "RON", "ARS", "HRK"
        };
    }
}