namespace InvestementsTracker.DTO;

    public record ReturnsDTO
    {
        public string Name { get; set; }
        public DateOnly PurchasedOn { get; set; }
        public double PurchasedAtPrice { get; set; }
        public string Change { get; set; }
    }

