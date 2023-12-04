
namespace Solution.csv_parsing
{
    internal class Transaction
    {
        public string InvestmentId { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public decimal Value { get; set; }

        public override string ToString()
        {
            return InvestmentId + " " + Type + " " + Date + " " + Value;
        }

    }
}
