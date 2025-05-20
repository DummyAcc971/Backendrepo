using MyStockSymbolApi.Models;
using MyStockSymbolApi.Repositories;
using MyStockSymbolApi.Services;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MyStockSymbolApi.Repositories
{
    public class GuestStockRepository : IGuestStockRepository
    {
        private readonly IGuestStockService _stockService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GuestStockRepository(IGuestStockService stockService, IHttpContextAccessor httpContextAccessor)
        {
            _stockService = stockService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<StockQuote?> GetGuestStockQuoteAsync(string symbol)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            int searchCount = session.GetInt32("GuestSearchCount") ?? 0;

            if (searchCount >= 2) 
            {
                return null; 
            }

            var result = await _stockService.GetGuestStockDataAsync(symbol);
            if (result == null) 
            {
                return null;
            }
            return result;
    }
}
}

