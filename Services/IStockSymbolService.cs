using System.Collections.Generic;
using System.Threading.Tasks;
using MyStockSymbolApi.Models;

namespace MyStockSymbolApi.Services
{
    public interface IStockSymbolService
    {
        Task<List<StockSymbol>> GetSuggestionsAsync(string query);
    }
}
