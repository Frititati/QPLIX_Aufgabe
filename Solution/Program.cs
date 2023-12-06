using Solution.csv_parsing;
using System.Diagnostics;

namespace Solution
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            CSVParser parser = new CSVParser();
            var s1 = Stopwatch.StartNew();
            List<Investment> investments = parser.parseInvestments("C:\\Users\\filip\\OneDrive\\Desktop\\QPLIX_Aufgabe\\Solution\\csv_file\\Investments.csv");
            s1.Stop();
            var s2 = Stopwatch.StartNew();
            List<Quote> quotes = parser.parseQuotes("C:\\Users\\filip\\OneDrive\\Desktop\\QPLIX_Aufgabe\\Solution\\csv_file\\Quotes.csv");
            s2.Stop();
            var s3 = Stopwatch.StartNew();
            List<Transaction> transactions = parser.parseTransactions("C:\\Users\\filip\\OneDrive\\Desktop\\QPLIX_Aufgabe\\Solution\\csv_file\\Transactions.csv");
            s3.Stop();
            //var s4 = Stopwatch.StartNew();
            //parser.parseTransactionsFaster("C:\\Users\\filip\\OneDrive\\Desktop\\QPLIX_Aufgabe\\Solution\\csv_file\\Transactions.csv");
            //s4.Stop();

            Console.WriteLine(s1.Elapsed.TotalMilliseconds);
            Console.WriteLine(s2.Elapsed.TotalMilliseconds);
            Console.WriteLine(s3.Elapsed.TotalMilliseconds);
            //Console.WriteLine(s4.Elapsed.TotalMilliseconds);

            Console.WriteLine("write: date;investorid");
            //var line = Console.ReadLine();
            var line = "16/03/2019;Investor1";
            while (!string.IsNullOrEmpty(line))
            {
                var input = line.Split(";");
                var date = DateTime.Parse(input[0]);
                var investorId = input[1];

                List<Investment> local_investments = investments.FindAll(t => t.InvestorId == investorId);

                // Start with finding out the value of all stocks
                List<Investment> stock_investments = local_investments.FindAll(t => t.InvestmentType == "Stock");
                List<StockValue> stock_value_list = new List<StockValue>();
                // TODO objects here could be null
                foreach (var item in stock_investments)
                {
                    // find all transactions that match this investments and respect the date limit
                    List<Transaction> local_transactions = transactions.FindAll(t => t.InvestmentId == item.InvestmentId && DateTime.Compare(t.Date, date) <= 0);
                    // calculate the number of shares
                    decimal number_of_shares = local_transactions.Sum(t => t.Value);
                    // find the last possible quotation of the stock price
                    Quote local_quote = quotes.FindLast(t => t.ISIN == item.ISIN && DateTime.Compare(t.Date, date) <= 0);

                    // add this stock to the total list of stocks
                    StockValue stock_value = new StockValue
                    {
                        ISIN = item.ISIN,
                        number_of_shared = number_of_shares,
                        price_per_share = local_quote.PricePerShare
                    };
                    stock_value_list.Add(stock_value);
                }

                // this prints out all the of the invested stocks (limited to the correct date) and their value
                //foreach(var item in stock_value_list)
                //{
                //    Console.WriteLine(item.ToString());
                //}

                // Here we calculate the RealEstate investment
                List<Investment> realestate_investments = local_investments.FindAll(t => t.InvestmentType == "RealEstate");
                List<RealEstateValue> realestate_value_list = new List<RealEstateValue>();
                foreach (var item in realestate_investments)
                {
                    // find all transactions that match this investments and respect the date limit
                    List<Transaction> local_transactions = transactions.FindAll(t => t.InvestmentId == item.InvestmentId && DateTime.Compare(t.Date, date) <= 0);
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
                    RealEstateValue real_estate_value = new RealEstateValue
                    {
                        name = item.City,
                        estate_initial_price = estate_inital_value,
                        building_initial_price = building_inital_value
                    };
                    
                    // use specialized functions to calculate estate and building updates
                    real_estate_value.SetUpdateEstate(estate_transactions);
                    real_estate_value.SetUpdateBuilding(building_transactions);

                    realestate_value_list.Add(real_estate_value);
                }

                foreach(var item in realestate_value_list)
                {
                    Console.WriteLine(item.ToString());
                }



                // Console.WriteLine("count " + stock_transactions.Count);

                //List<Investment> fonds = ins.FindAll(t => t.InvestmentType == "Fonds");

                //foreach (var fund in fonds)
                //{
                //    List<Investment> iss = investments.FindAll(t => t.InvestorId == fund.FondsInvestor);
                //    foreach (var issue in iss)
                //    {
                //        Console.WriteLine(issue.ToString());
                //    }
                //}

                //foreach (var investment in ins)
                //{
                //    Console.WriteLine(investment.ToString());
                //}

                // List<Transaction> trans = transaction.FindAll(t => t.);

                Console.WriteLine("results!");
                // line = Console.ReadLine();
                line = "";
            }


        }
    }
}
