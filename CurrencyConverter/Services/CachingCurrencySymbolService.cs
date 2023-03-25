using System.Collections;

namespace CurrencyConverter.Services;

public class CachingCurrencySymbolService : ICurrencySymbolService
{
    private readonly ICurrencySymbolService _innerService;
    private readonly Hashtable _cache;

    public CachingCurrencySymbolService(ICurrencySymbolService innerService)
    {
        _innerService = innerService;
        _cache = new Hashtable();
    }

    public string[] GetCurrencySymbols()
    {
        const string cacheKey = "CurrencySymbols";

        if (!_cache.ContainsKey(cacheKey))
        {
            var symbols = _innerService.GetCurrencySymbols();
            _cache[cacheKey] = symbols;
        }

        return (string[])_cache[cacheKey];
    }
}
