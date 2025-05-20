using System.Collections.Generic;
using System.Threading.Tasks;
using MyStockSymbolApi.Models;

namespace MyStockSymbolApi.Data
{
    public interface IStockSymbolRepository
    {
        Task<List<StockSymbol>> GetSymbolsAsync();
    }
}
