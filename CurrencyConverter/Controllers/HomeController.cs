using System.Text.Json;
using CurrencyConverter.Configurations;
using CurrencyConverter.Dto;
using Microsoft.AspNetCore.Mvc;
using CurrencyConverter.Models;
using Microsoft.Extensions.Options;

namespace CurrencyConverter.Controllers;

public class HomeController : Controller
{
    private const string DefaultTargetCurrency = "USD";
    private const string DefaultSourceCurrency = "EUR";
    private const int InitialInputAmount = 100;
    private readonly ApiConfig _apiConfig;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public HomeController(IOptions<ApiConfig> apiConfig)
    {
        _apiConfig = apiConfig.Value;
    }

    public IActionResult Index()
    {
        return View(new CurrencyConversionViewModel
        {
            TargetCurrency = DefaultTargetCurrency,
            SourceCurrency = DefaultSourceCurrency,
            InputAmount = InitialInputAmount,
            CurrencySymbols = GetCurrencySymbols(_apiConfig.ApiKey)
        });
    }

    [HttpPost]
    public IActionResult ConvertCurrency(CurrencyConversionViewModel model)
    {
        var apiKey = _apiConfig.ApiKey;
        var rate = GetExchangeRate(model, apiKey);
        model.ConvertedAmount = rate;
        model.CurrencySymbols = GetCurrencySymbols(_apiConfig.ApiKey);

        return View("Index", model);
    }

    private static double GetExchangeRate(CurrencyConversionViewModel model, string apiKey)
    {
        //https://exchangeratesapi.io/
        var apiUrl = $"https://api.apilayer.com/exchangerates_data/convert?to={model.TargetCurrency}&from={model.SourceCurrency}&amount={model.InputAmount}";

        var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);

        request.Headers.Add("apikey", apiKey);

        var response = new HttpClient().Send(request);

        if (!response.IsSuccessStatusCode) throw new Exception($"Error fetching exchange rate: {response.ReasonPhrase}");
        var exchangeRateResponse = JsonSerializer.Deserialize<ExchangeRateResponse>(response.Content.ReadAsStringAsync().Result, JsonOptions); // TODO: make this async

        return exchangeRateResponse!.Result;
    }

    private static string[] GetCurrencySymbols(string apiKey)
    {
        //https://exchangeratesapi.io/
        const string apiUrl = "https://api.apilayer.com/exchangerates_data/symbols";

        var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
        request.Headers.Add("apikey", apiKey);

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

    public IActionResult Error()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
}
