using Microsoft.AspNetCore.Mvc;
using MyStockSymbolApi.Services.Interfaces;

namespace MyStockSymbolApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpGet("{symbol}")]
        public async Task<IActionResult> Get(string symbol)
        {
            try
            {
                var result = await _stockService.GetStockDataAsync(symbol);
                if (result == null)
                    return NotFound($"Stock data not found for symbol: {symbol}");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
