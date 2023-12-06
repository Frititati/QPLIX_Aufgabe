using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution
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
            return "Name: " + ISIN + "; Number of Shares: " + number_of_shared + "; Price per Share: " + price_per_share + "; Total Value: " + TotalValue();
        }
    }
}
