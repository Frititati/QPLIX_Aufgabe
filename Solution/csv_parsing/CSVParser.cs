﻿using Microsoft.VisualBasic.FileIO;


namespace Solution.csv_parsing
{
    internal class CSVParser
    {
        public List<Transaction> parseTransactions(string filePath)
        {
            // prepare the return object
            List<Transaction> result = new List<Transaction>();

            // using TextFieldParser as it is part of the common Microsoft.VisualBasic package
            // potentially here could have used CSVHelper but it not part of the common package
            using (TextFieldParser parser = new TextFieldParser(filePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");

                // This skips the headers of the CSV File
                // It explictly reads the first line without parsing it
                if (!parser.EndOfData)
                {
                    parser.ReadLine();
                }

                // check if the end of file has been reached
                while (!parser.EndOfData)
                {
                    // split the individual field of the row
                    string[] row = parser.ReadFields();

                    // added a try/catch to make sure if there is something the doesn't match
                    // Assumption 1, the program doesn't break unexpectedly
                    try
                    {
                        // create a new object of Transaction and place all values from row inside the object
                        Transaction transaction = new Transaction
                        {
                            // InvestmentId = row[0],
                            Type = row[1],
                            Date = DateTime.Parse(row[2]),
                            Value = Decimal.Parse(row[3])
                        };
                        result.Add(transaction);
                    }
                    catch (Exception ex)
                    {
                        // TODO: Improve error message
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            return result;
        }

        public List<Transaction> parseTransactionsFaster(string filePath)
        {
            // prepare the return object
            List<Transaction> result = new List<Transaction>();

            // uses string Split() method, much faster the TextFieldParser
            using (StreamReader reader = new StreamReader(filePath))
            {
                char[] delimiters = new char[] { ';' };
                reader.ReadLine();

                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                    {
                        break;
                    }

                    string[] row = line.Split(delimiters);

                    // added a try/catch to make sure if there is something the doesn't match
                    // Assumption 1, the program doesn't break unexpectedly
                    try
                    {
                        // create a new object of Transaction and place all values from row inside the object
                        Transaction transaction = new Transaction
                        {
                            // InvestmentId = row[0],
                            Type = row[1],
                            Date = DateTime.Parse(row[2]),
                            Value = Decimal.Parse(row[3])
                        };
                        result.Add(transaction);
                    }
                    catch (Exception ex)
                    {
                        // TODO: Improve error message
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            return result;
        }

        public Dictionary<string, List<Transaction>> parseTransactionsFasterDictionary(string filePath)
        {
            // prepare the return object
            Dictionary<string, List<Transaction>> result = new();

            // uses string Split() method, much faster the TextFieldParser
            using (StreamReader reader = new StreamReader(filePath))
            {
                char[] delimiters = new char[] { ';' };
                reader.ReadLine();

                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                    {
                        break;
                    }

                    string[] row = line.Split(delimiters);

                    // added a try/catch to make sure if there is something the doesn't match
                    // Assumption 1, the program doesn't break unexpectedly
                    try
                    {
                        // create a new object of Transaction and place all values from row inside the object
                        Transaction transaction = new Transaction
                        {
                            Type = row[1],
                            Date = DateTime.Parse(row[2]),
                            Value = Decimal.Parse(row[3])
                        };

                        if (result.ContainsKey(row[0]))
                        {
                            // update the existing entry
                            result[row[0]].Add(transaction);
                        }
                        else
                        {
                            // create new entry
                            result[row[0]] = new List<Transaction> { transaction };

                        }
                    }
                    catch (Exception ex)
                    {
                        // TODO: Improve error message
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            return result;
        }

        public List<Quote> parseQuotes(string filePath)
        {
            // prepare the return object
            List<Quote> result = new List<Quote>();

            // using TextFieldParser as it is part of the common Microsoft.VisualBasic package
            // potentially here could have used CSVHelper but it not part of the common package
            using (TextFieldParser parser = new TextFieldParser(filePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");

                // This skips the headers of the CSV File
                // It explictly reads the first line without parsing it
                if (!parser.EndOfData)
                {
                    parser.ReadLine();
                }

                // check if the end of file has been reached
                while (!parser.EndOfData)
                {
                    // split the individual field of the row
                    string[] row = parser.ReadFields();

                    // added a try/catch to make sure if there is something the doesn't match
                    // Assumption 1, the program doesn't break unexpectedly
                    try
                    {
                        // create a new object of Quote and place all values from row inside the object
                        Quote quote = new Quote
                        {
                            // ISIN = row[0],
                            Date = DateTime.Parse(row[1]),
                            PricePerShare = Decimal.Parse(row[2])
                        };
                        result.Add(quote);
                    }
                    catch (Exception ex)
                    {
                        // TODO: Improve error message
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            return result;
        }

        public List<Quote> parseQuotesFaster(string filePath)
        {
            // prepare the return object
            List<Quote> result = new List<Quote>();

            // uses string Split() method, much faster the TextFieldParser
            using (StreamReader reader = new StreamReader(filePath))
            {
                char[] delimiters = new char[] { ';' };
                reader.ReadLine();

                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                    {
                        break;
                    }

                    string[] row = line.Split(delimiters);

                    // added a try/catch to make sure if there is something the doesn't match
                    // Assumption 1, the program doesn't break unexpectedly
                    try
                    {
                        // create a new object of Transaction and place all values from row inside the object
                        Quote quote = new Quote
                        {
                            // ISIN = row[0],
                            Date = DateTime.Parse(row[1]),
                            PricePerShare = Decimal.Parse(row[2])
                        };
                        result.Add(quote);
                    }
                    catch (Exception ex)
                    {
                        // TODO: Improve error message
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            return result;
        }

        public Dictionary<string, List<Quote>> parseQuotesFasterDictionary(string filePath)
        {
            // prepare the return object
            Dictionary<string, List<Quote>> result = new();

            // uses string Split() method, much faster the TextFieldParser
            using (StreamReader reader = new StreamReader(filePath))
            {
                char[] delimiters = new char[] { ';' };
                reader.ReadLine();

                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                    {
                        break;
                    }

                    string[] row = line.Split(delimiters);

                    // added a try/catch to make sure if there is something the doesn't match
                    // Assumption 1, the program doesn't break unexpectedly
                    try
                    {
                        // create a new object of Transaction and place all values from row inside the object
                        Quote quote = new Quote
                        {
                            // ISIN = row[0],
                            Date = DateTime.Parse(row[1]),
                            PricePerShare = Decimal.Parse(row[2])
                        };

                        if (result.ContainsKey(row[0]))
                        {
                            // update the existing entry
                            result[row[0]].Add(quote);
                        }
                        else
                        {
                            // create new entry
                            result[row[0]] = new List<Quote> { quote };
                        }
                    }
                    catch (Exception ex)
                    {
                        // TODO: Improve error message
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            return result;
        }

        public List<Investment> parseInvestments(string filePath)
        {
            // prepare the return object
            List<Investment> result = new List<Investment>();

            // using TextFieldParser as it is part of the common Microsoft.VisualBasic package
            // potentially here could have used CSVHelper but it not part of the common package
            using (TextFieldParser parser = new TextFieldParser(filePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");

                // This skips the headers of the CSV File
                // It explictly reads the first line without parsing it
                if (!parser.EndOfData)
                {
                    parser.ReadLine();
                }

                // check if the end of file has been reached
                while (!parser.EndOfData)
                {
                    // split the individual field of the row
                    string[] row = parser.ReadFields();

                    // added a try/catch to make sure if there is something the doesn't match
                    // Assumption 1, the program doesn't break unexpectedly
                    try
                    {
                        // create a new object of Investment and place all values from row inside the object
                        Investment investment = new Investment
                        {
                            // InvestorId = row[0],
                            InvestmentId = row[1],
                            InvestmentType = row[2],
                            ISIN = row[3],
                            City = row[4],
                            FondsInvestor = row[5],
                        };
                        result.Add(investment);
                    }
                    catch (Exception ex)
                    {
                        // TODO: Improve error message
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            return result;
        }

        public List<Investment> parseInvestmentsFaster(string filePath)
        {
            // prepare the return object
            List<Investment> result = new List<Investment>();

            // uses string Split() method, much faster the TextFieldParser
            using (StreamReader reader = new StreamReader(filePath))
            {
                char[] delimiters = new char[] { ';' };
                reader.ReadLine();

                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                    {
                        break;
                    }

                    string[] row = line.Split(delimiters);

                    // added a try/catch to make sure if there is something the doesn't match
                    // Assumption 1, the program doesn't break unexpectedly
                    try
                    {
                        // create a new object of Transaction and place all values from row inside the object
                        Investment investment = new Investment
                        {
                            // InvestorId = row[0],
                            InvestmentId = row[1],
                            InvestmentType = row[2],
                            ISIN = row[3],
                            City = row[4],
                            FondsInvestor = row[5],
                        };
                        result.Add(investment);
                    }
                    catch (Exception ex)
                    {
                        // TODO: Improve error message
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            return result;
        }
        public Dictionary<string, List<Investment>> parseInvestmentsFasterDictionary(string filePath)
        {
            // prepare the return object
            Dictionary<string, List<Investment>> result = new();

            // uses string Split() method, much faster the TextFieldParser
            using (StreamReader reader = new StreamReader(filePath))
            {
                char[] delimiters = new char[] { ';' };
                reader.ReadLine();

                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                    {
                        break;
                    }

                    string[] row = line.Split(delimiters);

                    // added a try/catch to make sure if there is something the doesn't match
                    // Assumption 1, the program doesn't break unexpectedly
                    try
                    {
                        // create a new object of Transaction and place all values from row inside the object
                        Investment investment = new Investment
                        {
                            // InvestorId = row[0],
                            InvestmentId = row[1],
                            InvestmentType = row[2],
                            ISIN = row[3],
                            City = row[4],
                            FondsInvestor = row[5],
                        };

                        if (result.ContainsKey(row[0]))
                        {
                            // update the existing entry
                            result[row[0]].Add(investment);
                        }
                        else
                        {
                            // create a new entry
                            result[row[0]] = new List<Investment> { investment };
                        }
                    }
                    catch (Exception ex)
                    {
                        // TODO: Improve error message
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            return result;
        }
    }
}
