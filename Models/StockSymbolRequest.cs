namespace MyStockSymbolApi.Models
{
    public class StockSymbolRequest
    {
        public List<string> StockSymbols { get; set; }

        // Initialize the list to avoid warnings
        public StockSymbolRequest()
        {
            StockSymbols = new List<string>();
        }
    }
}
