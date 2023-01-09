namespace InvestementsTracker.InPzuDatabase;

    public class InPzuResult : IEquatable<InPzuResult>
    {
        public long Id { get; set; }
        public long PositionId { get; set; }
        public DateOnly Date { get; set; }
        public double Value { get; set; }
        public double Percentile { get; set; }

        public InPzuPosition Position { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is InPzuResult result ? Equals(result) : false;
        }

        public bool Equals(InPzuResult? other)
        {
            return other != null && PositionId == other.PositionId && Date == other.Date;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Date, this.PositionId);
        }
    }

