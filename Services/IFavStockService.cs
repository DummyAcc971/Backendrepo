using System.Collections.Generic;
using System.Threading.Tasks;
using MyStockSymbolApi.Models;

namespace MyStockSymbolApi.Services
{
    public interface IFavStockService
    {
        Task<List<StockData>> FetchStockDataForMultipleSymbolsAsync(List<string> stockSymbols);
    }
}
