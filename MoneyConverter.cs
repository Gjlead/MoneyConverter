using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyConverter
{

    // TO DO: change "million" to "millionen" once we have two or more millions
    internal class MoneyConverter
    {
        static Dictionary<int, string> getNumberStringDict = new Dictionary<int, string>()
        {
            { 1, "ein" }, { 2, "zwei" }, { 3, "drei" }, { 4, "vier" }, { 5, "fünf" }, { 6, "sechs" }, { 7, "sieben" }, { 8, "acht" }, { 9, "neun" }, { 10, "zehn" },
            { 11, "elf" }, { 12, "zwölf" }, { 20, "zwanzig" }, { 30, "dreißig" }, { 40, "vierzig" }, { 50, "fünfzig" }, { 60, "sechzig" }, { 70, "siebzig" },
            { 80, "achtzig" }, { 90, "neunzig" }, { 100, "hundert" }, { 1000, "tausend" }, { 1000000, "million" }, { 2000000, "millionen" }
        };

        static Dictionary<string, int> getNumberIntDict = new Dictionary<string, int>();

        static void Main(string[] args)
        {
            // Populate reverse dictionary
            foreach (var entry in getNumberStringDict)
            {
                getNumberIntDict[entry.Value] = entry.Key;
            }

            Console.WriteLine(ConvertMoney(123000002.23m));
        }

        static string ConvertMoney(decimal amount)
        {
            // EURO
            int euro = SplitIntoEuroAndCent(amount)[0];
            string euroString = "";
            if (euro == 0) { }
            if (euro > 999999999)
            {
                Console.WriteLine("Bitte einen 1- bis 9-stelligen Eurobetrag eingeben.");
                Environment.Exit(1);
            }
            else
            {
                // Split euro amount into blocks of up to 3 digits (in blocks of hundreds)
                int[] euroBlocks = SplitNumberIntoBlocks(euro);

                // Look up the correct number words for each hundreds block individually and add "hundert", "tausend" or "million" at the end if needed
                for (int i = 0; i < euroBlocks.Length; i++)
                {
                    if (euroBlocks[i] == 0)
                    {
                        continue;
                    }
                    else if (euroBlocks[i] > 0 && euroBlocks[i] < 13)
                    {
                        euroString += getNumberStringDict[euroBlocks[i]];
                    }
                    else
                    {
                        if (euroBlocks[i] > 99)
                        {
                            euroString += getNumberStringDict[euroBlocks[i] / 100] + getNumberStringDict[100];
                            euroBlocks[i] = euroBlocks[i] % 100;
                        }
                        if (euroBlocks[i] > 0 && euroBlocks[i] < 13)
                        {
                            euroString += getNumberStringDict[euroBlocks[i]];
                        }
                        else
                        {
                            if (euroBlocks[i] % 10 > 0)
                            {
                                // Take the last number first and add "und" before adding the second to last number
                                euroString += getNumberStringDict[euroBlocks[i] % 10] + "und";
                                euroString += getNumberStringDict[euroBlocks[i] - euroBlocks[i] % 10];
                            }
                            else
                            {
                                euroString += getNumberStringDict[euroBlocks[i]];
                            }
                        }
                    }
                    // Add "thousand" or "million" to the word if neccessary
                    if (euroBlocks.Length - i == 3)
                    {
                        euroString += getNumberStringDict[1000000];
                    }
                    else if (euroBlocks.Length - i == 2)
                    {
                        euroString += getNumberStringDict[1000];
                    }
                }
                euroString += " Euro";
            }
            


            // CENT
            int cent = SplitIntoEuroAndCent(amount)[1];
            string centString = "";
            if (cent == 0) { }
            if (cent > 99)
            {
                Console.WriteLine("Bitte maximal 99 Cent eingeben.");
                Environment.Exit(1);
            }
            else
            {
                if (cent < 13)
                {
                    centString += getNumberStringDict[cent];
                }
                else
                {
                    if (cent % 10 > 0)
                    {
                        centString += getNumberStringDict[cent % 10] + "und";
                        centString += getNumberStringDict[cent - cent % 10];
                    }
                    else
                    {
                        centString += getNumberStringDict[cent];
                    }
                }
                centString += " Cent";
            }

            return euroString + " " + centString;
        }


        static int[] SplitIntoEuroAndCent(decimal amount)
        {
            string money = "";

            try
            {
                money = amount.ToString();
            }
            catch (FormatException)
            {
                Console.WriteLine("Bitte einen Geldwert in Form von 1 bis 9 Euro-Ziffern und exakt 2 Cent-Ziffern eingeben, getrennt durch einen Punkt.");
                Environment.Exit(1);
            }

            string[] euroAndCent = money.Split(',');

            return new int[] { Int32.Parse(euroAndCent[0]), Int32.Parse(euroAndCent[1]) };
        }

        // Splits the number into blocks of 3
        static int[] SplitNumberIntoBlocks(int amount)
        {
            string money = amount.ToString();
            int amountOfBlocks = (int)Math.Ceiling((double)money.Length / 3);
            int[] blocks = new int[amountOfBlocks];

            for (int i = amountOfBlocks-1; i > 0; i--)
            {
                blocks[i] = Int32.Parse(money.Substring(i*3, 3));
            }

            if (money.Length%3 == 0)
            {
                blocks[0] = Int32.Parse(money.Substring(0, 3));
            }
            else
            {
                blocks[0] = Int32.Parse(money.Substring(0, money.Length % 3)); // Get the first incomplete block of less than 3 digits
            }

            return blocks;
        }


        static int ReverseConvert(string amount)
        {
            string euroString = "";
            string centString = "";

            if (amount.Contains("Euro"))
            {
                euroString = amount.Split(new string[] {"Euro"}, StringSplitOptions.None)[0];
                // StringSplitOptions.TrimEntries would have dealt with white-spaces, but that option is only available since .NET 5, and this solution targets the 4.8 framework.
                euroString.Trim();
            }

            List<string> blocks = new List<string>();
            if (euroString.Contains("million"))
            {
                blocks.Add(euroString.Split(new String[] { "million" }, StringSplitOptions.None)[0]);
                euroString = euroString.Split(new String[] { "million" }, StringSplitOptions.None)[1];
            }
            else if (euroString.Contains("millionen"))
            {
                blocks.Add(euroString.Split(new String[] { "millionen" }, StringSplitOptions.None)[0]);
                euroString = euroString.Split(new String[] { "millionen" }, StringSplitOptions.None)[1];
            }
            if (euroString.Contains("tausend"))
            {
                blocks.Add(euroString.Split(new String[] { "tausend" }, StringSplitOptions.None)[0]);
                euroString = euroString.Split(new String[] { "tausend" }, StringSplitOptions.None)[1];
            }
            // Add an empty thousands block for numbers such as zweimillionenfünf where the thousands block is left out in the word.
            else if(blocks.Count > 0)
            {
                blocks.Add("");
            }
            blocks.Add(euroString);




            if (amount.Contains("Cent"))
            {
                centString = amount.Split(new string[] { "Cent" }, StringSplitOptions.None)[0];
                centString.Trim();
            }

            return 1;
        }
    }
}
