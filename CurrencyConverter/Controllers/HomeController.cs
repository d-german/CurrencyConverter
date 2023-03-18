using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using CurrencyConverter.Models;

namespace CurrencyConverter.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View(new CurrencyConversion
            {
                TargetCurrency = "USD",
                SourceCurrency = "EUR",
                InputAmount = 100
            });
        }

        [HttpPost]
        public async Task<IActionResult> ConvertCurrency(CurrencyConversion model)
        {
            if (ModelState.IsValid)
            {
                var apiKey = "wvRLMpGapsw9N1GVwB2E1MlPC35XgSVj"; // Replace this with your actual API key
                var rate = await GetExchangeRateAsync(model, apiKey);
                model.ConvertedAmount = rate;
            }

            return View("Index", model);
        }

        private async Task<double> GetExchangeRateAsync(CurrencyConversion model, string apiKey)
        {
            using var httpClient = new HttpClient();

            var apiUrl = $"https://api.apilayer.com/exchangerates_data/convert?to={model.TargetCurrency}&from={model.SourceCurrency}&amount={model.InputAmount}";

            var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);

            request.Headers.Add("apikey", apiKey);

            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(responseContent);
                return (double)json["result"];
            }

            throw new Exception($"Error fetching exchange rate: {response.ReasonPhrase}");
        }
    }
}
