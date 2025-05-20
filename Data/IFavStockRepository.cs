using System.Collections.Generic;
using System.Threading.Tasks;
using MyStockSymbolApi.Models;

namespace MyStockSymbolApi.Repositories
{
    public interface IFavStockRepository
    {
        Task<List<StockData>> GetStockQuoteAsync(List<string> symbols);
    }
}
