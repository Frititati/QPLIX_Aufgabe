using Solution.csv_parsing;
using System.Diagnostics;

namespace Solution
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CSVParser parser = new();
            Console.WriteLine("Loading CSV files");
            var s1 = Stopwatch.StartNew();
            Dictionary<string, List<Investment>> all_investments = parser.parseInvestmentsFasterDictionary("C:\\Users\\filip\\OneDrive\\Desktop\\QPLIX_Aufgabe\\Solution\\csv_file\\Investments.csv");
            Dictionary<string, List<Quote>> all_quotes = parser.parseQuotesFasterDictionary("C:\\Users\\filip\\OneDrive\\Desktop\\QPLIX_Aufgabe\\Solution\\csv_file\\Quotes.csv");
            Dictionary<string, List<Transaction>> all_transactions = parser.parseTransactionsFasterDictionary("C:\\Users\\filip\\OneDrive\\Desktop\\QPLIX_Aufgabe\\Solution\\csv_file\\Transactions.csv");
            s1.Stop();

            Console.WriteLine("Finished loading CSV files in: " + s1.Elapsed.TotalMilliseconds + "ms");

            Console.WriteLine("write: date;investorid");
            //var line = Console.ReadLine();
            var line = "16/03/2019;Investor1";
            while (!string.IsNullOrEmpty(line))
            {
                var s2 = Stopwatch.StartNew();
                var input = line.Split(";");
                var selected_date = DateTime.Parse(input[0]);
                var selected_investorId = input[1];

                List<Investment> filtered_investments = all_investments[selected_investorId];

                List<StockValue> stock_value_list = ProcessStockInvestments(all_transactions, all_quotes, filtered_investments, selected_date);

                // this prints out all the of the invested stocks (limited to the correct date) and their value
                decimal stock_total_value = 0;
                foreach (var item in stock_value_list)
                {
                    Console.WriteLine(item.ToString());
                    stock_total_value += item.TotalValue();
                }

                // Here we calculate the RealEstate investment
                List<RealEstateValue> realestate_value_list = ProcessRealEstateInvestments(all_transactions, filtered_investments, selected_date);

                decimal realestate_total_value = 0;
                foreach (var item in realestate_value_list)
                {
                    Console.WriteLine(item.ToString());
                    realestate_total_value += item.TotalValue();
                }

                List<FondValue> fond_value_list = ProcessFondValue(all_transactions, all_investments, all_quotes, filtered_investments, selected_date);

                decimal fond_total_value = 0;
                foreach (var item in fond_value_list)
                {
                    Console.WriteLine(item.ToString());
                    fond_total_value += item.TotalValue();
                }

                Console.WriteLine("Stock Total value: " + Decimal.Round(stock_total_value, 2));
                Console.WriteLine("RealEstate Total value: " + Decimal.Round(realestate_total_value, 2));
                Console.WriteLine("Fond Total value: " + Decimal.Round(fond_total_value, 2));
                s2.Stop();
                Console.WriteLine("Execution took: " + s2.Elapsed.TotalMilliseconds + "ms");
                // line = Console.ReadLine();
                line = "";
            }
        }
        private static List<StockValue> ProcessStockInvestments(Dictionary<string, List<Transaction>> all_transactions, Dictionary<string, List<Quote>> all_quotes, List<Investment> investments, DateTime selected_date)
        {
            // Start with finding out the value of all stocks
            List<Investment> stock_investments = investments.FindAll(t => t.InvestmentType == "Stock");
            // the return List<StockValue>
            List<StockValue> stock_value_list = new();

            // TODO objects here could be null
            foreach (var investment in stock_investments)
            {
                // find all transactions that match this investments and respect the date limit
                List<Transaction> local_transactions = all_transactions[investment.InvestmentId].FindAll(t => DateTime.Compare(t.Date, selected_date) <= 0);
                // calculate the number of shares
                decimal number_of_shares = local_transactions.Sum(t => t.Value);
                // find the last possible quotation of the stock price
                Quote local_quote = all_quotes[investment.ISIN].FindLast(t => DateTime.Compare(t.Date, selected_date) <= 0);

                // add this stock to the total list of stocks
                StockValue stock_value = new()
                {
                    ISIN = investment.ISIN,
                    number_of_shared = number_of_shares,
                    price_per_share = local_quote.PricePerShare
                };
                stock_value_list.Add(stock_value);
            }
            return stock_value_list;
        }

        private static List<RealEstateValue> ProcessRealEstateInvestments(Dictionary<string, List<Transaction>> all_transactions, List<Investment> investments, DateTime selected_date)
        {
            // Here we calculate the RealEstate investment
            List<Investment> realestate_investments = investments.FindAll(t => t.InvestmentType == "RealEstate");
            // the return List<RealEstateValue>
            List<RealEstateValue> realestate_value_list = new();

            // TODO : obj here could be null
            foreach (var investment in realestate_investments)
            {
                // find all transactions that match this investments and respect the date limit
                List<Transaction> local_transactions = all_transactions[investment.InvestmentId].FindAll(t => DateTime.Compare(t.Date, selected_date) <= 0);
                // get the individual estate transactions
                List<Transaction> estate_transactions = local_transactions.FindAll(t => t.Type == "Estate");
                // get the individual building transactions
                List<Transaction> building_transactions = local_transactions.FindAll(t => t.Type == "Building");

                // By assumption 2: if there is no estate transaction there is no building transaction
                // skip and don't process this investement (because investments haven't happened yet, due to the date contraint)
                if (estate_transactions.Count == 0 || building_transactions.Count == 0)
                {
                    continue;
                }

                // here we know these values will not be out of bounds
                Decimal estate_inital_value = estate_transactions[0].Value;
                Decimal building_inital_value = building_transactions[0].Value;

                // create the RealEstateValue object
                RealEstateValue real_estate_value = new()
                {
                    name = investment.City,
                    estate_initial_price = estate_inital_value,
                    building_initial_price = building_inital_value
                };

                // use specialized functions to calculate estate and building updates
                real_estate_value.SetUpdateEstate(estate_transactions);
                real_estate_value.SetUpdateBuilding(building_transactions);

                realestate_value_list.Add(real_estate_value);
            }
            return realestate_value_list;
        }

        private static List<FondValue> ProcessFondValue(Dictionary<string, List<Transaction>> all_transactions, Dictionary<string, List<Investment>> all_investments, Dictionary<string, List<Quote>> all_quotes, List<Investment> filtered_investments, DateTime selected_date)
        {
            List<FondValue> fond_value_list = new();
            List<Investment> investment_in_fond = filtered_investments.FindAll(t => t.InvestmentType == "Fonds");
            foreach (var investment in investment_in_fond)
            {
                // calculate percentage invested in fond
                List<Transaction> local_transactions = all_transactions[investment.InvestmentId].FindAll(t => DateTime.Compare(t.Date, selected_date) <= 0);
                decimal percentage_owned = local_transactions.Sum(t => t.Value);

                if (percentage_owned == 0)
                {
                    continue;
                }

                // calculate the value of the fond
                List<Investment> iss = all_investments[investment.FondsInvestor];
                List<StockValue> fond_stock_value_list = ProcessStockInvestments(all_transactions, all_quotes, iss, selected_date);
                List<RealEstateValue> fond_realestate_value_list = ProcessRealEstateInvestments(all_transactions, iss, selected_date);

                decimal fond_stock_total_value = 0;
                foreach (var item in fond_stock_value_list)
                {
                    fond_stock_total_value += item.TotalValue();
                }
                decimal fond_realestate_total_value = 0;
                foreach (var item in fond_realestate_value_list)
                {
                    fond_realestate_total_value += item.TotalValue();
                }

                FondValue fond_value = new FondValue
                {
                    name = investment.FondsInvestor,
                    stock_value = fond_stock_total_value,
                    real_estate_value = fond_realestate_total_value,
                    percentage_owned = percentage_owned
                };

                fond_value_list.Add(fond_value);
            }
            return fond_value_list;
        }
    }
}
