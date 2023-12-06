# QPLIX_Aufgabe: Investment Analysis Tool

## Description
This tool provides a comprehensive analysis of investments based on CSV data files. It processes data for different investment types like stocks, real estate, and fonds, calculating their values as of a specified date.

## Features
- Parses CSV files for investments, quotes, and transactions.
- Provides detailed calculations for the value of stock, real estate, and fund investments.
- Customizable output with a verbose option for detailed logging. (changable on line 11 of Program.cs)
- Efficient data handling using dictionaries and lists for fast access times.

## Assumptions Taken
- Assumption 1:
	The CSV file is formatted always correctly, always Delimited with ";", always follows the same type throught the file.
- Assumption 2:
	Whenever an Estate is bought there is always a Building transaction as well, on the same date, and vice versa.
- Assumption 3:
	The date provided in the input string, is just a date without time.
- Assumption 4:
	A Fond does not invest in another fond (found from data analysis), only Stock and/or Real Estate.
- Assumption 5:
	The value are in Euro, but I didn't put the euro sign before numbers.

## Usage
Run the program and follow the on-screen instructions. You will be prompted to enter a date and an investor ID in the format `<date>;<investorid>`. The tool will then display the total value of the investments for the specified investor as of the given date.

## Data Error
For some investments, it could happen that a transaction for a stock goes throught before an available quotation, here the stock is not taken into account. Example of data inconsistency: Investment21067.
Here Investment21067 describes that 144.88 shares are bought at the 12/01/2016 however the first available quotation for ISIN70 is the 28/06/2016.