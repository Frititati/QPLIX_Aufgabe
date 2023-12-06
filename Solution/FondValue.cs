using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution
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
            return "Name: " + name + ";\tRealEstate Value: " + Decimal.Round(real_estate_value, 2)
                + ";\tStock Value: " + Decimal.Round(stock_value, 2) + ";\tPercentage Owned: "
                + Decimal.Round(percentage_owned, 3) + ";\tTotal Value: " + Decimal.Round(TotalValue(), 2);
        }
    }
}
