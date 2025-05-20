using MyStockSymbolApi.Models;
using System.Threading.Tasks;

namespace MyStockSymbolApi.Services
{
    public interface IGuestStockService
    {
        Task<StockQuote?> GetGuestStockDataAsync(string symbol);
    }
}
