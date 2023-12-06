
namespace Solution.value_objects
{
    internal class FondValue
    {
        public string name { get; set; }
        public decimal real_estate_value { get; set; }
        public decimal stock_value { get; set; }
        public decimal percentage_owned { get; set; }

        // here its possible to make a List<StockValue> and List<RealEstateValue> much like in Program.cs, however I didn't because of performance concerns
        public decimal TotalValue()
        {
            return (real_estate_value + stock_value) * percentage_owned;
        }
        public override string ToString()
        {
            return "Name: " + name + ";\tRealEstate Value: " + decimal.Round(real_estate_value, 2)
                + ";\tStock Value: " + decimal.Round(stock_value, 2) + ";\tPercentage Owned: "
                + decimal.Round(percentage_owned, 3) + ";\tTotal Value: " + decimal.Round(TotalValue(), 2);
        }
    }
}
