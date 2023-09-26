using System.Text.Json;
using CurrencyConverter.Configurations;
using CurrencyConverter.Dto;
using Microsoft.Extensions.Options;

namespace CurrencyConverter.Services;

public class CurrencySymbolService : ICurrencySymbolService
{
    private readonly ApiConfig _apiConfig;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public CurrencySymbolService(IOptions<ApiConfig> apiConfig)
    {
        _apiConfig = apiConfig.Value;
    }

    public string[] GetCurrencySymbols()
    {
        return GetCurrencySymbolsInternal();
    }

    private string[] GetCurrencySymbolsInternal()
    {
        //https://exchangeratesapi.io/
        const string apiUrl = "https://api.apilayer.com/exchangerates_data/symbols";

        var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
        request.Headers.Add("apikey", _apiConfig.ApiKey);

        var response = new HttpClient().Send(request);

        if (!response.IsSuccessStatusCode) throw new Exception($"Error fetching exchange rate: {response.ReasonPhrase}");

        var exchangeRateResponse = JsonSerializer.Deserialize<CurrencySymbolsResponse>(response.Content.ReadAsStringAsync().Result, JsonOptions); // TODO: make this async

        // TODO: use linq
        var keyArray = new string[exchangeRateResponse.Symbols.Keys.Count];
        var i = 0;
        foreach (var key in exchangeRateResponse.Symbols.Keys)
        {
            keyArray[i++] = (string)key;
        }

        return keyArray;
    }
}