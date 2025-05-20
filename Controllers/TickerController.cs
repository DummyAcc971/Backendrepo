using Microsoft.AspNetCore.Mvc;
using MyStockSymbolApi.Services;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MyStockSymbolApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TickerController : ControllerBase
    {
        private readonly ITickerService _tickerService;

        public TickerController(ITickerService tickerService)
        {
            _tickerService = tickerService;
        }

        [HttpGet("trending")]
        public async Task<IActionResult> GetTrendingTickers()
        {
            try
            {
                var tickers = await _tickerService.GetTrendingTickersAsync();

                if (tickers == null || !tickers.Any())
                {
                    return NotFound(new
                    {
                        title = "No trending tickers found.",
                        status = 404
                    });
                }

                var filteredTickers = tickers.Select(t => new
                {
                    t.Symbol,
                    t.LongName,
                    t.RegularMarketPrice,
                    t.RegularMarketChangePercent,
                    t.TrendingScore,
                    t.QuoteType
                });

                return Ok(filteredTickers);
            }
            catch (HttpRequestException httpEx) // Handles API call failures
            {
                return StatusCode((int)HttpStatusCode.ServiceUnavailable, new
                {
                    title = "External API error",
                    status = 503,
                    details = httpEx.Message
                });
            }
            catch (TimeoutException timeoutEx) // Handles timeout issues
            {
                return StatusCode((int)HttpStatusCode.RequestTimeout, new
                {
                    title = "Request timed out",
                    status = 408,
                    details = timeoutEx.Message
                });
            }
            catch (UnauthorizedAccessException authEx) // Handles unauthorized access issues
            {
                return StatusCode((int)HttpStatusCode.Unauthorized, new
                {
                    title = "Unauthorized access",
                    status = 401,
                    details = authEx.Message
                });
            }
            catch (InvalidOperationException invalidOpEx) // Handles logical errors
            {
                return StatusCode((int)HttpStatusCode.BadRequest, new
                {
                    title = "Invalid operation",
                    status = 400,
                    details = invalidOpEx.Message
                });
            }
            catch (Exception ex) // Handles unexpected exceptions
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
