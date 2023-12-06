
namespace Solution.csv_parsing
{
    internal class Transaction
    {
        // Didn't change the value names to make them standardized with, kept them like in the csv

        // the InvestmentId initially was part of the class but was moved to the key of the dictionary, for efficiency reasons
        // public string InvestmentId { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public decimal Value { get; set; }

        public override string ToString()
        {
            // return InvestmentId + " " + Type + " " + Date + " " + Value;
            return Type + " " + Date + " " + Value;
        }

    }
}
