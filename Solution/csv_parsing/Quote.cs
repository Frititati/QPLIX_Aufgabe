
namespace Solution.csv_parsing
{
    internal class Quote
    {
        public string ISIN { get; set; }
        public DateTime Date { get; set; }
        public decimal PricePerShare { get; set; }

        public override string ToString()
        {
            return ISIN + " " + Date + " " + PricePerShare;
        }
    }
}
