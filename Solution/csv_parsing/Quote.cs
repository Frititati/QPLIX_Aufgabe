namespace Solution.csv_parsing
{
    internal class Quote
    {
        // Didn't change the value names to make them standardized with, kept them like in the csv

        // the ISIN initially was part of the class but was moved to the key of the dictionary, for efficiency reasons
        // public string ISIN { get; set; }
        public DateTime Date { get; set; }
        public decimal PricePerShare { get; set; }

        public override string ToString()
        {
            // return ISIN + " " + Date + " " + PricePerShare;
            return Date + " " + PricePerShare;
        }
    }
}
