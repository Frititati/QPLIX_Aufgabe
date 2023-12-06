using Solution.csv_parsing;

namespace Solution
{
    internal class RealEstateValue
    {
        // Decided to do a proper breakdown of the realestate value such that it could be used in the future for other things
        public string name { get; set; }
        public decimal building_initial_price { get; set; }
        public decimal building_update_price { get; set; }
        public decimal estate_initial_price { get; set; }
        public decimal estate_update_price { get; set; }
        public void SetUpdateBuilding(List<Transaction> transactions)
        {
            building_update_price = 0;
            // have to skip the first value of the list as its the initial price
            foreach (var transaction in transactions.Skip(1))
            {
                building_update_price += transaction.Value;
            }
        }
        public void SetUpdateEstate(List<Transaction> transactions)
        {
            estate_update_price = 0;
            // have to skip the first value of the list as its the initial price
            foreach (var transaction in transactions.Skip(1))
            {
                estate_update_price += transaction.Value;
            }
        }
        public decimal TotalValue()
        {
            return building_initial_price + building_update_price + estate_initial_price + estate_update_price;
        }
        public override string ToString()
        {
            return "Name: " + name + ";\tBuilding Initial Value: " + Decimal.Round(building_initial_price) + ";\tBuilding Updates in Value: "
                + Decimal.Round(building_update_price, 2) + ";\tEstate Initial Value: " + Decimal.Round(estate_initial_price, 2)
                + ";\tEstate Updates in Price: " + Decimal.Round(estate_update_price, 2) + ";\tTotal Value: " + Decimal.Round(TotalValue(), 2);
        }
    }
}
