using MyStockSymbolApi.Models;
using MyStockSymbolApi.Services.Interfaces;
using System.Text.Json;

namespace MyStockSymbolApi.Services
{
    public class StockService : IStockService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public StockService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<string?> GetStockTickerAsync(string symbol)
        {
            try
            {
                var apiKey = _configuration["Finnhub:ApiKey"];
                var baseUrl = "https://finnhub.io/api/v1/stock/profile2";

                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"{baseUrl}?symbol={symbol}&token={apiKey}");

                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"API request failed with status code: {response.StatusCode}");

                var json = await response.Content.ReadFromJsonAsync<JsonElement>();
                return json.TryGetProperty("ticker", out JsonElement ticker) ? ticker.GetString() : null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching ticker: {ex.Message}");
                return null;
            }
        }

        public async Task<StockQuote?> GetStockDataAsync(string symbol)
        {
            try
            {
                var ticker = await GetStockTickerAsync(symbol);
                if (string.IsNullOrEmpty(ticker))
                    throw new InvalidOperationException($"Ticker not found for symbol: {symbol}");

                var apiKey = _configuration["Finnhub:ApiKey"];
                var baseUrl = _configuration["Finnhub:BaseUrl"];
                
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"{baseUrl}?symbol={ticker}&token={apiKey}");

                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"Stock data API request failed: {response.StatusCode}");

                var json = await response.Content.ReadFromJsonAsync<JsonElement>();

                decimal current = json.GetProperty("c").GetDecimal();
                decimal open = json.GetProperty("o").GetDecimal();
                decimal high = json.GetProperty("h").GetDecimal();
                decimal low = json.GetProperty("l").GetDecimal();
                decimal prevClose = json.GetProperty("pc").GetDecimal();

                decimal change = current - prevClose;
                decimal percent = prevClose == 0 ? 0 : (change / prevClose) * 100;
                decimal progress = (high - low) == 0 ? 0 : (current - low) / (high - low) * 100;

                return new StockQuote(ticker)
                {
                    CurrentPrice = current,
                    Open = open,
                    High = high,
                    Low = low,
                    PreviousClose = prevClose,
                    Change = change,
                    PercentChange = percent,
                    Progress = progress
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving stock data: {ex.Message}");
                return null;
            }
        }
    }
}
