namespace InvestementsTracker.InPzuDatabase;

    public class InPzuPortfolio
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateOnly Since { get; set; }
        public ICollection<InPzuPosition> Positions { get; set; }
    }
