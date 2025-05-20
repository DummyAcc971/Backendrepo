using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyStockSymbolApi.Repositories;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace MyStockSymbolApi.Controllers
{
    [ApiController]
    [Route("api/stock")]
    public class FavStockController : ControllerBase
    {
        private readonly IFavStockRepository _repository;
        private readonly IConfiguration _configuration;

        public FavStockController(IFavStockRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        [HttpGet("fetch")]
        public async Task<IActionResult> GetStocks([FromQuery] string symbols)
        {
            try
            {
                string apiKey = _configuration["TwelveData:ApiKey"] ?? throw new UnauthorizedAccessException("API Key is missing.");
                Console.WriteLine($"Fetched API Key in Controller: {apiKey}");

                var symbolList = symbols?.Split(',').ToList() ?? throw new ArgumentException("Symbols parameter is required.");
                Console.WriteLine($"Parsed Symbols List: {string.Join(", ", symbolList)}");

                var result = await _repository.GetStockQuoteAsync(symbolList);

                if (result == null || result.Count == 0)
                    throw new InvalidOperationException("No stock data found.");

                Console.WriteLine($"FavStockController Response Count: {result.Count}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
                Environment.Exit(0); // Ensures that file unlocking happens
                return StatusCode(500, new { message = $"An unexpected error occurred: {ex.Message}" });
            }
        }
    }
}
