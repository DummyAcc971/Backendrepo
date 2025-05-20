using System.Net.Http;
using System.Text.Json;
using MyStockSymbolApi.Models;
 
namespace MyStockSymbolApi.Services
{
   public class TickerService : ITickerService
{
    private readonly HttpClient _httpClient;
 
    public TickerService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }
 
    public async Task<List<StockSymbolss>> GetTrendingTickersAsync()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://yahoo-finance-real-time1.p.rapidapi.com/market/get-trending-tickers?region=US"),
        };
 
        request.Headers.Add("x-rapidapi-key", "ab099b30cfmshab875e1b376a285p12aad5jsn903b2d1ddc66");
        request.Headers.Add("x-rapidapi-host", "apidojo-yahoo-finance-v1.p.rapidapi.com");
 
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
 
        var json = await response.Content.ReadAsStringAsync();
        Console.WriteLine("RAW JSON:\n" + json);
 
        var result = JsonSerializer.Deserialize<TrendingTickersResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
 
        // Extract the required fields
        return result?.Finance?.Result?.FirstOrDefault()?.Quotes?.Select(q => new StockSymbolss
        {
            Symbol = q.Symbol,
            LongName=q.LongName,
            RegularMarketPrice = q.RegularMarketPrice,
            RegularMarketChangePercent = q.RegularMarketChangePercent,
            TrendingScore = q.TrendingScore,
            QuoteType = q.QuoteType
        }).ToList() ?? new List<StockSymbolss>();
    }
}
}   