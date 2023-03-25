using System.Text.Json;
using CurrencyConverter.Configurations;
using CurrencyConverter.Dto;
using Microsoft.Extensions.Options;

namespace CurrencyConverter.Services;

public class ExchangeRateService : IExchangeRateService
{
    private readonly ApiConfig _apiConfig;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public ExchangeRateService(IOptions<ApiConfig> apiConfig)
    {
        _apiConfig = apiConfig.Value;
    }

    public double GetExchangeRate(ExchangeRateRequest exchangeRateRequest)
    {
        //https://exchangeratesapi.io/
        var apiUrl = $"https://api.apilayer.com/exchangerates_data/convert?to={exchangeRateRequest.TargetCurrency}&from={exchangeRateRequest.SourceCurrency}&amount={exchangeRateRequest.InputAmount}";

        var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);

        request.Headers.Add("apikey", _apiConfig.ApiKey);

        var response = new HttpClient().Send(request);

        if (!response.IsSuccessStatusCode) throw new Exception($"Error fetching exchange rate: {response.ReasonPhrase}");
        var exchangeRateResponse = JsonSerializer.Deserialize<ExchangeRateResponse>(response.Content.ReadAsStringAsync().Result, JsonOptions); // TODO: make this async

        return exchangeRateResponse!.Result;
    }
}
