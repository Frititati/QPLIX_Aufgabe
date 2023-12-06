namespace Solution.csv_parsing
{
    internal class Investment
    {
        // Didn't change the value names to make them standardized with the other classes, kept them like in the csv
        public string InvestorId { get; set; }
        public string InvestmentId { get; set; }
        public string InvestmentType { get; set; }
        public string ISIN { get; set; }
        public string City { get; set; }
        public string FondsInvestor { get; set; }

        public override string ToString()
        {
            return InvestorId + " " + InvestmentId + " " + InvestmentType + " " + ISIN + " " + City + " " + FondsInvestor;
        }
    }
}
