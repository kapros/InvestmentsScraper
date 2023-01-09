namespace InvestementsTracker.InPzuDatabase;

    public class Cash
    {
        public int Value { get; set; }

        public string Currency { get; set; }

        public int InPln { get; set; }

        public Cash(int value, string currency)
        {
            Value = value;
            Currency = currency;
            InPln = (int)(currency.Equals("EUR", System.StringComparison.OrdinalIgnoreCase) ? value * 4.5d : currency.Equals("USD", System.StringComparison.OrdinalIgnoreCase) ? value * 4.0d : currency.Equals("GBP", System.StringComparison.OrdinalIgnoreCase) ? 5.0d : value);
        }

        public Cash(string x)
        {
            var split = x.Split("|");
            Value = int.Parse(split[0]);
            Currency = split[1];
            InPln = int.Parse(split[2]);
        }
    }

