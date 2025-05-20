using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyStockSymbolApi.Services;
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace MyStockSymbolApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StockSymbolsController : ControllerBase
    {
        private readonly IStockSymbolService _service;

        public StockSymbolsController(IStockSymbolService service)
        {
            _service = service;
        }

        // GET: api/StockSymbols/suggestions?query=A
        ///
        /// <summary>
        /// Get stock symbol suggestions based on a query string.
        /// </summary>
        /// <param name="query">The query string to search for stock symbols.</param>
        [HttpGet("suggestions")]
        public async Task<IActionResult> GetSuggestions([FromQuery] string query)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    return BadRequest(new
                    {
                        title = "The query parameter is required.",
                        status = 400
                    });
                }

                var suggestions = await _service.GetSuggestionsAsync(query);

                if (suggestions == null || suggestions.Count == 0)
                {
                    return NotFound(new
                    {
                        title = $"No suggestions found for query: '{query}'",
                        status = 404
                    });
                }

                return Ok(suggestions);
            }
            catch (HttpRequestException httpEx)
            {
                return StatusCode((int)HttpStatusCode.ServiceUnavailable, new
                {
                    title = "External API error",
                    status = 503,
                    details = httpEx.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    title = "An unexpected error occurred",
                    status = 500,
                    details = ex.Message
                });
            }
        }
    }
}
