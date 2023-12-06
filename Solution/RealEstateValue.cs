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
            foreach (var transaction in transactions.Skip(1))
            {
                building_update_price += transaction.Value;
            }
            //if (transactions.Count > 1)
            //{
            //    building_update_price = (List<decimal>)transactions.Skip(1).Select(t => t.Value);
            //    Console.WriteLine("we were here");
            //}
        }
        public void SetUpdateEstate(List<Transaction> transactions)
        {
            estate_update_price = 0;
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
            return "Name: " + name + "; Building Initial Price: " + building_initial_price + "; Building Updates in Price: " + building_update_price + "; Estate Initial Price: " + estate_initial_price + "; Estate Updates in Price: " + estate_update_price + "; Total Value: " + TotalValue();
        }
    }
}
