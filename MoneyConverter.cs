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
            { 11, "elf" }, { 12, "zwölf" }, { 13, "dreizehn"}, { 14, "vierzehn"}, { 15, "fünfzehn"}, { 16, "sechzehn"}, { 17, "siebzehn"}, { 18, "achtzehn"}, { 19, "neunzehn"},
            { 20, "zwanzig" }, { 30, "dreißig" }, { 40, "vierzig" }, { 50, "fünfzig" }, { 60, "sechzig" }, { 70, "siebzig" },
            { 80, "achtzig" }, { 90, "neunzig" }, { 100, "hundert" }, { 1000, "tausend" }, { 1000000, "million" }, { 2000000, "millionen" }
        };

        static Dictionary<string, int> getNumberIntDict = new Dictionary<string, int>();

        static void Main(string[] args)
        {
            // Populate reverse dictionary
            foreach (var entry in getNumberStringDict)
            {
                if (entry.Value == "hundert" || entry.Value == "tausend" || entry.Value.Contains("million")){
                    getNumberIntDict[entry.Value] = -1;
                }
                else
                {
                    getNumberIntDict[entry.Value] = entry.Key;
                }
            }
/*
            Console.WriteLine(ConvertMoney(123000002.23m));
            Console.WriteLine(ConvertMoney(123456789.23m));
            Console.WriteLine(ConvertMoney(41.14m));
            Console.WriteLine(ReverseConvert("einhundertdreiundzwanzig Euro zweiundzwanzig Cent"));
            Console.WriteLine(ReverseConvert("einhundertdreiundzwanzigtausend Euro zweiundzwanzig Cent"));
            Console.WriteLine(ReverseConvert("einhundertdreiundzwanzigmillion Euro fünfzehn Cent"));*/
            //Console.WriteLine(ReverseConvert("dreihundert Euro fünfzehn Cent"));
            //Console.WriteLine(ReverseConvert("einhundertdreiundzwanzigmilliondreihundert Euro fünfzehn Cent"));
            Console.WriteLine(ReverseConvert("einhundertdreiundzwanzigmilliondreihundertachtunddreißigtausendneununddreißig Euro fünfzehn Cent")); //BUGGY
            //Console.WriteLine(ReverseConvert("einhundertdreiundzwanzigtausendfünfhundertvierzehn Euro zweiundzwanzig Cent"));
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
                    else if (euroBlocks[i] > 0 && euroBlocks[i] < 20)
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
                        if (euroBlocks[i] > 0 && euroBlocks[i] < 20)
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
                if (cent < 20)
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

            // Split the number into 3-digit-blocks of hundreds
            List<string> blocks = new List<string>();
            if (euroString.Contains("million"))
            {
                blocks.Add(euroString.Split(new String[] { "million" }, StringSplitOptions.None)[0]);
                euroString = euroString.Split(new String[] { "million" }, StringSplitOptions.None)[1].Trim();
            }
            else if (euroString.Contains("millionen"))
            {
                blocks.Add(euroString.Split(new String[] { "millionen" }, StringSplitOptions.None)[0]);
                euroString = euroString.Split(new String[] { "millionen" }, StringSplitOptions.None)[1].Trim();
            }
            if (euroString.Contains("tausend"))
            {
                blocks.Add(euroString.Split(new String[] { "tausend" }, StringSplitOptions.None)[0]);
                euroString = euroString.Split(new String[] { "tausend" }, StringSplitOptions.None)[1].Trim();
            }
            // Add an empty thousands block for numbers such as zweimillionenfünf where the thousands block is left out in the word.
            // Situation: We have not encountered a "tausend", but have at least one block already, meaning we are in the millions and have to add the empty thousands block.
            else if(blocks.Count > 0)
            {
                blocks.Add("000");
            }
            // Situation: We have encountered million or tausend and nothing afterwards: We need to add some more zeroes.
            if (string.IsNullOrEmpty(euroString))
            {
                blocks.Add("000");
            }
            // Add the rest of the number, if there is a rest.
            else
            {
                blocks.Add(euroString);
            }

            string numberBuilder = "";
            int currentNumber;
            bool switchNumbers = false;

            for (int i = 0; i < blocks.Count; i++)
            {
                Console.WriteLine("builder: " + numberBuilder + " Block: " + blocks[i]);
                if (blocks[i] == "000")
                {
                    numberBuilder += "000";
                    continue;
                }
                // When looking for the numbers in the long word I can start at the third position because no number word is shorter than 3 characters
                int j = 3;
                while (j <= blocks[i].Length)
                {
                    // Prevent numbers like vierzehn from being split into 4 and 10
                    if (blocks[i].Length >= 8 && (blocks[i].Substring(0, j) == "drei" || blocks[i].Substring(0, j) == "vier" || blocks[i].Substring(0, j) == "fünf" ||
                        blocks[i].Substring(0, j) == "acht" || blocks[i].Substring(0, j) == "neun") && blocks[i].Substring(4, 4) == "zehn")
                    {
                        j += 4; // To also get the "zehn" coming after the number
                    }
                    try
                    {
                        currentNumber = getNumberIntDict[blocks[i].Substring(0, j)];
                        // Words like "tausend", "hundert" only interest us in edge cases like "dreihundert" where the number ends with them, because we then have to pad some 0s.
                        if (currentNumber == -1)
                        {
                            if (j >= blocks[i].Length - 1)
                            {
                                while (numberBuilder.Length % 3 > 0)
                                {
                                    numberBuilder += "0";
                                }
                            }
                        }
                        else
                        {
                            // Switch the last two numbers if they are connected by "und"
                            if (switchNumbers)
                            {
                                Console.WriteLine("Switch: " + numberBuilder);
                                int tempNumber = Int32.Parse(numberBuilder.Substring(numberBuilder.Length - 1));
                                numberBuilder = numberBuilder.Remove(numberBuilder.Length - 1);
                                numberBuilder += currentNumber/10 + "" + tempNumber;
                                Console.WriteLine("Switch done: " + numberBuilder);
                                switchNumbers = false;
                            }
                            else
                            {
                                numberBuilder += currentNumber;
                            }
                        }
                        blocks[i] = blocks[i].Substring(j); // Cut away the already converted part of the number
                        j = 3;

                    }
                    catch (KeyNotFoundException)
                    {
                        if (blocks[i].Substring(0, j) == "und")
                        {
                            switchNumbers = true;
                            blocks[i] = blocks[i].Substring(j);
                            j = 3;
                            continue;
                            
                        }
                        j++;
                        continue;
                    }
                }
            }



            if (amount.Contains("Cent"))
            {
                centString = amount.Split(new string[] { "Cent" }, StringSplitOptions.None)[0];
                centString.Trim();
            }

            return Int32.Parse(numberBuilder);
        }
    }
}
