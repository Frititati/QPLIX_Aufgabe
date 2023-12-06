namespace Solution.value_objects
{
    internal class StockValue
    {
        // Decided to do a proper breakdown of the stock value such that it could be used in the future for other things
        public string ISIN { get; set; }
        public decimal number_of_shared { get; set; }
        public decimal price_per_share { get; set; }
        public decimal TotalValue()
        {
            return number_of_shared * price_per_share;
        }
        public override string ToString()
        {
            return "Name: " + ISIN + ";\tNumber of Shares: " + number_of_shared + ";\tPrice per Share: "
                + price_per_share + ";\tTotal Value: " + decimal.Round(TotalValue(), 2);
        }

        // potentially one can be interested in the average price of purchase, this was not implemented for effiency reasons
    }
}
