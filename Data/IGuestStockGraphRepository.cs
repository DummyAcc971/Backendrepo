using MyStockSymbolApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyStockSymbolApi.Repositories
{
    public interface IGuestStockGraphRepository
    {
        Task<IEnumerable<StockDataPoint>> GetStockDataAsync(string symbol);
    }
}
