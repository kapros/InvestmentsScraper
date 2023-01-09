namespace InvestementsTracker.InPzuDatabase;

    public class InPzuPosition
    {
        public long Id { get; set; }
        public double Units { get; set; }
        public double PriceOfUnit { get; set; }
        public double PurchaseValue { get; set; }
        public string FundId { get; set; }
        public string RegistrationId { get; set; }
        public string FundName { get; set; }
        public DateOnly PurchaseDate { get; set; }
        public string UnitType { get; set; }
        public InPzuPortfolio Portfolio { get; set; }
        public long PortfolioId { get; set; }
        public ICollection<InPzuResult> Results { get; set; } = new List<InPzuResult>();
        public string Currency { get; set; }
    }
