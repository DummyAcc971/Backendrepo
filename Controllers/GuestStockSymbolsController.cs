using Microsoft.AspNetCore.Mvc;
using MyStockSymbolApi.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyStockSymbolApi.Services;

namespace MyStockSymbolApi.Controllers
{
    [ApiController]
    [Route("api/gueststocksymbols")]
    public class GuestStockSymbolsController : ControllerBase
    {
        private readonly IGuestStockSymbolService _service;

        public GuestStockSymbolsController(IGuestStockSymbolService service)
        {
            _service = service;
        }

        [HttpGet("suggestions")]
        public async Task<IActionResult> GetGuestStockSuggestions([FromQuery] string query)
        {
            var result = await _service.GetGuestStockSuggestionsAsync(query);
            return Ok(result);
        }
    }
}
