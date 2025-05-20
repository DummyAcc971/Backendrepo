using MyStockSymbolApi.Repositories;
using MyStockSymbolApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyStockSymbolApi.Services
{
    public class GuestStockGraphService : IGuestStockGraphService
    {
        private readonly IGuestStockGraphRepository _stockGraphRepository;

        public GuestStockGraphService(IGuestStockGraphRepository stockGraphRepository)
        {
            _stockGraphRepository = stockGraphRepository;
        }

        public async Task<IEnumerable<StockDataPoint>> GetStockDataAsync(string symbol)
        {
            return await _stockGraphRepository.GetStockDataAsync(symbol);
        }
    }
}
