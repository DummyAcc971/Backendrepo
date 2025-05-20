using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using MyStockSymbolApi.Models;

namespace MyStockSymbolApi.Data
{
    public class StockSymbolRepository : IStockSymbolRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private const string FinnhubUrl = "https://finnhub.io/api/v1/stock/symbol?exchange=US&token=cvsc161r01qvc2n02ms0cvsc161r01qvc2n02msg";
        private const string CacheKey = "StockSymbols";

        public StockSymbolRepository(HttpClient httpClient, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _cache = cache;
        }

        public async Task<List<StockSymbol>> GetSymbolsAsync()
        {
            if (_cache.TryGetValue(CacheKey, out List<StockSymbol> cachedSymbols))
            {
                return cachedSymbols;
            }

            var symbols = new List<StockSymbol>();

            using var responseStream = await _httpClient.GetStreamAsync(FinnhubUrl);
            var asyncEnumerable = JsonSerializer.DeserializeAsyncEnumerable<StockSymbol>(
                responseStream,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            await foreach (var stock in asyncEnumerable)
            {
                if (stock != null)
                {
                    symbols.Add(stock);
                }
            }

            // Store in cache for 4 hours
            _cache.Set(CacheKey, symbols, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(4)
            });

            return symbols;
        }
    }
}