using System.Collections;
using CurrencyConverter.Configurations;
using CurrencyConverter.Dto;
using Microsoft.AspNetCore.Mvc;
using CurrencyConverter.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CurrencyConverter.Controllers;

public class HomeController : Controller
{
    private readonly ApiConfig _apiConfig;

    public HomeController(IOptions<ApiConfig> apiConfig)
    {
        _apiConfig = apiConfig.Value;
    }

    public IActionResult Index()
    {
        return View(new CurrencyConversionViewModel
        {
            TargetCurrency = "USD",
            SourceCurrency = "EUR",
            InputAmount = 100,
            CurrencySymbols = GetCurrencySymbolsAsync(_apiConfig.ApiKey)
        });
    }

    [HttpPost]
    public IActionResult ConvertCurrency(CurrencyConversionViewModel model)
    {
        var apiKey = _apiConfig.ApiKey;
        var rate = GetExchangeRateAsync(model, apiKey);
        model.ConvertedAmount = rate;
        model.CurrencySymbols = GetCurrencySymbolsAsync(_apiConfig.ApiKey);

        return View("Index", model);
    }

    private static double GetExchangeRateAsync(CurrencyConversionViewModel model, string apiKey)
    {
        //https://exchangeratesapi.io/
        var apiUrl = $"https://api.apilayer.com/exchangerates_data/convert?to={model.TargetCurrency}&from={model.SourceCurrency}&amount={model.InputAmount}";

        var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);

        request.Headers.Add("apikey", apiKey);

        var response = new HttpClient().Send(request);

        if (!response.IsSuccessStatusCode) throw new Exception($"Error fetching exchange rate: {response.ReasonPhrase}");
        var exchangeRateResponse = JsonConvert.DeserializeObject<ExchangeRateResponse>(response.Content.ReadAsStringAsync().Result); //TODO: make this async

        return exchangeRateResponse!.Result;
    }

    private static string[] GetCurrencySymbolsAsync(string apiKey)
    {
        //https://exchangeratesapi.io/
        const string apiUrl = "https://api.apilayer.com/exchangerates_data/symbols";

        var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
        request.Headers.Add("apikey", apiKey);

        var response = new HttpClient().Send(request);

        if (!response.IsSuccessStatusCode) throw new Exception($"Error fetching exchange rate: {response.ReasonPhrase}");
        var exchangeRateResponse = JsonConvert.DeserializeObject<CurrencySymbolsResponse>(response.Content.ReadAsStringAsync().Result); //TODO: make this async

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
