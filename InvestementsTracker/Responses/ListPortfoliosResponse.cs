namespace InvestementsTracker.Responses;

public class ListPortfoliosResponse
{
    public string Name { get; internal set; }
    public IEnumerable<InPzuPositionResponse> Positions { get; internal set; }
    public double SumPurchased { get; internal set; }
    public double TotalChange { get; internal set; }
}

public class InPzuPositionResponse
{
    public string FundName { get; set; }
    public double SumOfPurchases { get; set; }
}
