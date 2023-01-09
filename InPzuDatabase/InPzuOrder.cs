namespace InvestementsTracker.InPzuDatabase;

    public class InPzuOrder : IEquatable<InPzuOrder>
    {
        public long Id { get; set; }
        public string OrderName { get; set; }
        public DateOnly PurchasedOn { get; set; }
        public int PurchaseWorth { get; set; }
        public string Currency { get; set; }
        public InPzuPortfolio Portfolio { get; set; }   
        public long PortfolioId { get; set; }
        public ICollection<InPzuPosition> Positions { get; set; }
        public OrderType OrderType { get; set; }

        public override int GetHashCode()
        {
            return OrderName.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;
            if (obj is InPzuOrder order)
                return Equals(order);
            return false;
        }

        public bool Equals(InPzuOrder? other)
        {
            return other != null && other.OrderName == OrderName;
        }
    }

    public enum OrderType
    {
        Purchase,
        Sale,
        Swap
    }

