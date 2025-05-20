using MyStockSymbolApi.Models;
using System.Threading.Tasks;

namespace MyStockSymbolApi.Repositories
{
    public interface IGuestStockRepository
    {
        Task<StockQuote?> GetGuestStockQuoteAsync(string symbol);
    }
}
