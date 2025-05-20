using System.Collections.Generic;
using System.Threading.Tasks;
using MyStockSymbolApi.Models;

namespace MyStockSymbolApi.Repositories
{
    public interface IGuestStockSymbolRepository
    {
        Task<List<StockSymbol>> GetGuestStockSymbolsAsync();
    }
}
