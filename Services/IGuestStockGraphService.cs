using MyStockSymbolApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyStockSymbolApi.Services
{
    public interface IGuestStockGraphService
    {
        Task<IEnumerable<StockDataPoint>> GetStockDataAsync(string symbol);
    }
}
