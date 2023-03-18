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
            InputAmount = 100
        });
    }

    [HttpPost]
    public IActionResult ConvertCurrency(CurrencyConversionViewModel model)
    {
        if (!ModelState.IsValid) return View("Index", model);

        var apiKey = _apiConfig.ApiKey;
        var rate = GetExchangeRateAsync(model, apiKey);
        model.ConvertedAmount = rate;

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
        var exchangeRateResponse = JsonConvert.DeserializeObject<ExchangeRateResponse>(response.Content.ReadAsStringAsync().Result); //note .result
        
        return exchangeRateResponse!.Result;
    }

    public IActionResult Privacy()
    {
        return View();
    }
}
