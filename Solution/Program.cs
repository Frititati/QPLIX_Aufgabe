using Solution.csv_parsing;
using Solution.value_objects;
using System.Diagnostics;

namespace Solution
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // change to true to see all components in details that make up the entire value of the portfolio at the chosen date
            bool verbose = false;

            CSVParser parser = new();
            Console.WriteLine("Loading CSV files");
            var s1 = Stopwatch.StartNew();

            Dictionary<string, List<Investment>> all_investments = new();
            Dictionary<string, List<Quote>> all_quotes = new();
            Dictionary<string, List<Transaction>> all_transactions = new();
            try
            {
                // fetch csv file data and place it into the dictionary <-> list structure for fast access times
                all_investments = parser.ParseInvestmentsDictionary(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "csv_file", "Investments.csv"));
                all_quotes = parser.ParseQuotesDictionary(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "csv_file", "Quotes.csv"));
                all_transactions = parser.ParseTransactionsDictionary(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "csv_file", "Transactions.csv"));
            }
            catch (Exception ex)
            {
                // catches any exceptions (expecting most FileNotFound) and halts the program
                Console.WriteLine(ex.Message);
                System.Environment.Exit(0);
            }
            s1.Stop();

            Console.WriteLine("Finished loading CSV files in: " + s1.Elapsed.TotalMilliseconds + "ms");

            Console.WriteLine("Please indicate the date and the investorId, like so: <date>;<investorid>");
            var line = Console.ReadLine();

            while (!string.IsNullOrEmpty(line))
            {
                // validate that the input data can be processed
                if (!ValidateInputData(line, all_investments.Keys.ToList()))
                {
                    Console.WriteLine("Please try again, like so: <date>;<investorid>");
                    line = Console.ReadLine();
                    continue;
                }
                var input = line.Split(";");
                var selected_date = DateTime.Parse(input[0]);
                var selected_investorId = input[1];

                // start stopwatch for execution time monitoring
                var s2 = Stopwatch.StartNew();

                List<Investment> filtered_investments_list = all_investments[selected_investorId];

                List<StockValue> stock_value_list = ProcessStockInvestments(all_transactions, all_quotes, filtered_investments_list, selected_date);

                // this prints out all the of the invested stocks (limited to the correct date) and their value
                decimal stock_total_value = 0;
                foreach (var item in stock_value_list)
                {
                    if (verbose) { Console.WriteLine(item.ToString()); }
                    stock_total_value += item.TotalValue();
                }

                // Here we calculate the RealEstate investment
                List<RealEstateValue> realestate_value_list = ProcessRealEstateInvestments(all_transactions, filtered_investments_list, selected_date);

                decimal realestate_total_value = 0;
                foreach (var item in realestate_value_list)
                {
                    if (verbose) { Console.WriteLine(item.ToString()); }
                    realestate_total_value += item.TotalValue();
                }

                List<FondValue> fond_value_list = ProcessFondValue(all_transactions, all_investments, all_quotes, filtered_investments_list, selected_date);

                decimal fond_total_value = 0;
                foreach (var item in fond_value_list)
                {
                    if (verbose) { Console.WriteLine(item.ToString()); }
                    fond_total_value += item.TotalValue();
                }

                Console.WriteLine(selected_investorId + " Summary:");
                Console.WriteLine("Stock Total value: " + Decimal.Round(stock_total_value, 2));
                Console.WriteLine("RealEstate Total value: " + Decimal.Round(realestate_total_value, 2));
                Console.WriteLine("Fond Total value: " + Decimal.Round(fond_total_value, 2));
                Console.WriteLine("Total value: " + Decimal.Round(stock_total_value + realestate_total_value + fond_total_value, 2));
                s2.Stop();
                Console.WriteLine("Execution took: " + s2.Elapsed.TotalMilliseconds + "ms");

                Console.WriteLine("Please write new entry, like so: <date>;<investorid>");
                line = Console.ReadLine();
            }
        }
        private static List<StockValue> ProcessStockInvestments(Dictionary<string, List<Transaction>> all_transactions, Dictionary<string, List<Quote>> all_quotes, List<Investment> investments, DateTime selected_date)
        {
            // Start with finding out the value of all stocks
            List<Investment> stock_investments = investments.FindAll(t => t.InvestmentType == "Stock");
            // the return List<StockValue>
            List<StockValue> stock_value_list = new();

            foreach (var investment in stock_investments)
            {
                // find all transactions that match this investments and respect the date limit
                List<Transaction> local_transactions = all_transactions[investment.InvestmentId].FindAll(t => DateTime.Compare(t.Date, selected_date) <= 0);

                // check that there are transactions
                if (local_transactions.Count == 0)
                {
                    continue;
                }

                // calculate the number of shares
                decimal number_of_shares = local_transactions.Sum(t => t.Value);
                // find the last possible quotation of the stock price
                Quote last_quote = all_quotes[investment.ISIN].FindLast(t => DateTime.Compare(t.Date, selected_date) <= 0);

                if (last_quote == null)
                {
                    // if the last quote is null mean there is no available price
                    // skip this transactions
                    continue;
                }

                // add this stock to the total list of stocks
                StockValue stock_value = new()
                {
                    ISIN = investment.ISIN,
                    number_of_shared = number_of_shares,
                    price_per_share = last_quote.PricePerShare
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
                decimal estate_inital_value = estate_transactions[0].Value;
                decimal building_inital_value = building_transactions[0].Value;

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

                // check that there are transactions or/and that the Fond is owned by the investor
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
                    // Console.WriteLine(item.ToString());
                    fond_stock_total_value += item.TotalValue();
                }
                decimal fond_realestate_total_value = 0;
                foreach (var item in fond_realestate_value_list)
                {
                    // Console.WriteLine(item.ToString());
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

        private static bool ValidateInputData(string input_data, List<string> valid_investors)
        {
            string[] parts = input_data.Split(';');

            if (parts.Length != 2)
            {
                Console.WriteLine("Missing one of the input parameters, please write <Date>;<InvestorId>");
                return false;
            }

            if (!DateTime.TryParse(parts[0], out _))
            {
                Console.WriteLine("Date inserted is not valid.");
                return false;
            }

            // check if the investor starts with Investor and is within the list of valid investors
            if (!parts[1].StartsWith("Investor") || !valid_investors.Contains(parts[1]))
            {
                Console.WriteLine("Investor inserted is not valid.");
                return false;
            }

            return true;
        }
    }
}
