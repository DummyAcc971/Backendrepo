public class TrendingTickersResponse
{
    public Finance Finance { get; set; }
}

public class Finance
{
    public List<Result> Result { get; set; }
}

public class Result
{
    public List<StockSymbolss> Quotes { get; set; }
}

public class StockSymbolss
{
    public string Symbol { get; set; }
    public string LongName { get; set; }
    public decimal RegularMarketPrice { get; set; }
    public decimal RegularMarketChangePercent { get; set; }
    public decimal TrendingScore { get; set; }
    public string QuoteType { get; set; }
}