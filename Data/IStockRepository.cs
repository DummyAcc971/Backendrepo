using MyStockSymbolApi.Models;

namespace MyStockSymbolApi.Repositories.Interfaces
{
    public interface IStockRepository
    {
        Task<StockQuote> GetStockQuoteAsync(string symbol);
    }
}
