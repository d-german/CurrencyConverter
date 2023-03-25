namespace CurrencyConverter.Services;

public interface IExchangeRateService
{
    double GetExchangeRate(ExchangeRateRequest exchangeRateRequest);
}