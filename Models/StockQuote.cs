namespace MyStockSymbolApi.Models
{
    public class StockQuote
    {
        public string Symbol { get; set; }  // Add this property
        public decimal CurrentPrice { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal PreviousClose { get; set; }
        public decimal Change { get; set; }
        public decimal PercentChange { get; set; }
        public decimal Progress { get; set; }

        // Add a constructor that accepts a symbol
        public StockQuote(string symbol)
        {
            Symbol = symbol;
        }
    }
}
