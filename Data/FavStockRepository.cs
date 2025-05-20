using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyStockSymbolApi.Models;
using MyStockSymbolApi.Services;

namespace MyStockSymbolApi.Repositories
{
    public class FavStockRepository : IFavStockRepository
    {
        private readonly IFavStockService _stockService;

        public FavStockRepository(IFavStockService stockService)
        {
            _stockService = stockService;
        }

        public async Task<List<StockData>> GetStockQuoteAsync(List<string> symbols)
        {
            Console.WriteLine($"Fetching live stock data for: {string.Join(", ", symbols)}");

            if (symbols == null || symbols.Count == 0)
            {
                Console.WriteLine("No symbols received by repository.");
                return new List<StockData>();
            }

            return await _stockService.FetchStockDataForMultipleSymbolsAsync(symbols);
        }
    }
}
