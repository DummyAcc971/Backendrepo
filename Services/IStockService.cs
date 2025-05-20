using MyStockSymbolApi.Models;

namespace MyStockSymbolApi.Services.Interfaces
{
    public interface IStockService
    {
        Task<StockQuote> GetStockDataAsync(string symbol);
    }
}
