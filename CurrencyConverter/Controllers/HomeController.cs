using Microsoft.AspNetCore.Mvc;
using CurrencyConverter.Models;
using CurrencyConverter.Services;

namespace CurrencyConverter.Controllers;

public class HomeController : Controller
{
    private const string DefaultTargetCurrency = "USD";
    private const string DefaultSourceCurrency = "EUR";
    private const int InitialInputAmount = 100;

    private readonly ICurrencySymbolService _currencySymbolService;
    private readonly IExchangeRateService _exchangeRateService;

    public HomeController(ICurrencySymbolService currencySymbolService, IExchangeRateService exchangeRateService)
    {
        _currencySymbolService = currencySymbolService;
        _exchangeRateService = exchangeRateService;
    }

    public IActionResult Index()
    {
        return View(new CurrencyConversionViewModel
        {
            TargetCurrency = DefaultTargetCurrency,
            SourceCurrency = DefaultSourceCurrency,
            InputAmount = InitialInputAmount,
            CurrencySymbols = _currencySymbolService.GetCurrencySymbols()
        });
    }

    [HttpPost]
    public IActionResult ConvertCurrency(CurrencyConversionViewModel model)
    {
        var rate = _exchangeRateService.GetExchangeRate(new ExchangeRateRequest
        {
            SourceCurrency = model.SourceCurrency,
            TargetCurrency = model.TargetCurrency,
            InputAmount = model.InputAmount
        });

        model.ConvertedAmount = rate;
        model.CurrencySymbols = _currencySymbolService.GetCurrencySymbols();

        return View("Index", model);
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
