using MyStockSymbolApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyStockSymbolApi.Services
{
    public interface IGuestStockSymbolService
    {
        Task<List<StockSymbol>> GetGuestStockSuggestionsAsync(string query);
    }
}
