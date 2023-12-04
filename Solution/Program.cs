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
            parser.parseInvestments("C:\\Users\\filip\\OneDrive\\Desktop\\QPLIX_Aufgabe\\Solution\\csv_file\\Investments.csv");
            s1.Stop();
            var s2 = Stopwatch.StartNew();
            parser.parseQuotes("C:\\Users\\filip\\OneDrive\\Desktop\\QPLIX_Aufgabe\\Solution\\csv_file\\Quotes.csv");
            s2.Stop();
            var s3 = Stopwatch.StartNew();
            parser.parseTransactions("C:\\Users\\filip\\OneDrive\\Desktop\\QPLIX_Aufgabe\\Solution\\csv_file\\Transactions.csv");
            s3.Stop();

            Console.WriteLine(s1.Elapsed.TotalMilliseconds);
            Console.WriteLine(s2.Elapsed.TotalMilliseconds);
            Console.WriteLine(s3.Elapsed.TotalMilliseconds);


        }
    }
}
