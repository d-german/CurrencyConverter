namespace CurrencyConverter.Services;

public class LocalExchangeRateService : IExchangeRateService
{
    private static readonly Random RandomGenerator = new Random();

    public double GetExchangeRate(ExchangeRateRequest exchangeRateRequest)
    {
        // Generate a random exchange rate between 0.5 and 1.5 for local development
        return Math.Round(RandomGenerator.NextDouble() + 0.5, 4);
    }
}