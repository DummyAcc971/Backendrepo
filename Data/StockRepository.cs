using MyStockSymbolApi.Models;
using MyStockSymbolApi.Repositories.Interfaces;
using MyStockSymbolApi.Services.Interfaces;

namespace MyStockSymbolApi.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly IStockService _stockService;

        public StockRepository(IStockService stockService)
        {
            _stockService = stockService;
        }

        public async Task<StockQuote> GetStockQuoteAsync(string symbol)
        {
            return await _stockService.GetStockDataAsync(symbol);
        }
    }
}
