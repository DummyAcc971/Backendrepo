using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using MyStockSymbolApi.Models;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyStockSymbolApi.Repositories
{
    public class GuestStockGraphRepository : IGuestStockGraphRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GuestStockGraphRepository(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["ApiKeys:TwelveData"] ?? throw new ArgumentNullException("API Key is missing in configuration.");
        }

        public async Task<IEnumerable<StockDataPoint>> GetStockDataAsync(string symbol)
        {
            string url = $"https://api.twelvedata.com/time_series?symbol={symbol}&interval=1min&outputsize=1440&apikey={_apiKey}";
            var response = await _httpClient.GetAsync(url);

            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);
            var values = json["values"]?.ToList();

            if (values == null || values.Count == 0)
            {
                throw new Exception("No data available for this symbol or invalid symbol.");
            }

            // Filter data for the last 24 hours
            var now = DateTime.UtcNow;
            var stockData = values
                .Where(item =>
                {
                    var datetime = DateTime.Parse(item["datetime"]?.ToString() ?? string.Empty, CultureInfo.InvariantCulture);
                    return datetime >= now.AddHours(-72); // Only include data from the last 24 hours
                })
                .Select(item => new StockDataPoint
                {
                    Datetime = item["datetime"]?.ToString(),
                    Price = double.Parse(item["close"]?.ToString() ?? "0", CultureInfo.InvariantCulture)
                })
                .ToList();

            return stockData;
        }
    }
}
