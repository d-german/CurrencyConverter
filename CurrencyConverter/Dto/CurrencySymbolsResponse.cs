using System.Collections;

namespace CurrencyConverter.Dto;

public class CurrencySymbolsResponse
{
    public bool Success { get; set; }
    public Hashtable Symbols { get; set; }
}