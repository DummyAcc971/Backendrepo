using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MyStockSymbolApi.Models;

namespace MyStockSymbolApi.Services
{
    public class FavStockService : IFavStockService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public FavStockService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["TwelveData:ApiKey"] ?? throw new ArgumentNullException(nameof(_apiKey), "API Key is missing.");
            _baseUrl = configuration["TwelveData:BaseUrl"] ?? throw new ArgumentNullException(nameof(_baseUrl), "Base URL is missing.");
        }

        public async Task<List<StockData>> FetchStockDataForMultipleSymbolsAsync(List<string> stockSymbols)
        {
            try
            {
                if (stockSymbols == null || stockSymbols.Count == 0)
                    throw new ArgumentException("Stock symbols list is empty.");

                Console.WriteLine($"Fetching stock data for symbols: {string.Join(", ", stockSymbols)}");

                string symbolsQuery = string.Join(",", stockSymbols);
                string url = $"{_baseUrl}/quote?symbol={symbolsQuery}&apikey={_apiKey}";

                using HttpResponseMessage response = await _httpClient.GetAsync(url);
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Response: {responseBody}");

                response.EnsureSuccessStatusCode(); // Handle API errors

                JsonDocument jsonDocument = JsonDocument.Parse(responseBody);
                List<StockData> stockList = new List<StockData>();

                foreach (var property in jsonDocument.RootElement.EnumerateObject())
                {
                    JsonElement stockElement = property.Value;

                    decimal closePrice = TryParseDecimal(stockElement, "close");
                    decimal percentChange = TryParseDecimal(stockElement, "percent_change");

                    stockList.Add(new StockData(
                        stockElement.GetProperty("symbol").GetString() ?? "N/A",
                        stockElement.GetProperty("name").GetString() ?? "Unknown",
                        closePrice,
                        percentChange
                    ));
                }

                Console.WriteLine($"Parsed Stock List: {stockList.Count} items fetched.");
                return stockList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
                return new List<StockData> { new StockData("Error", "An unexpected error occurred", 0, 0) };
            }
        }

        private decimal TryParseDecimal(JsonElement element, string propertyName)
        {
            if (element.TryGetProperty(propertyName, out var prop) && decimal.TryParse(prop.GetString(), out var parsedValue))
            {
                return parsedValue;
            }
            return 0; // Return default value if parsing fails
        }
    }
}
