﻿using System;
using System.Collections.Generic;

namespace MoneyConverter
{
    internal class MoneyConverter
    {
        static Dictionary<int, string> getNumberStringDict = new Dictionary<int, string>()
        {
            { 1, "ein" }, { 2, "zwei" }, { 3, "drei" }, { 4, "vier" }, { 5, "fünf" }, { 6, "sechs" }, { 7, "sieben" }, { 8, "acht" }, { 9, "neun" }, { 10, "zehn" },
            { 11, "elf" }, { 12, "zwölf" }, { 13, "dreizehn"}, { 14, "vierzehn"}, { 15, "fünfzehn"}, { 16, "sechzehn"}, { 17, "siebzehn"}, { 18, "achtzehn"}, { 19, "neunzehn"},
            { 20, "zwanzig" }, { 30, "dreißig" }, { 40, "vierzig" }, { 50, "fünfzig" }, { 60, "sechzig" }, { 70, "siebzig" },
            { 80, "achtzig" }, { 90, "neunzig" }, { 100, "hundert" }, { 1000, "tausend" }, { 1000000, "million" }
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

            Console.WriteLine("123000002.23 = " + ConvertMoney(123000002.23m));
            Console.WriteLine("123456789.23 = " + ConvertMoney(123456789.23m));
            Console.WriteLine("41.14 = " + ConvertMoney(41.14m));
            Console.WriteLine("vierzehn Euro = " + ReverseConvert("vierzehn Euro"));
            Console.WriteLine("vierzehn Cent = " + ReverseConvert("vierzehn Cent"));
            Console.WriteLine("einhundertdreiundzwanzig Euro zweiundzwanzig Cent = " + ReverseConvert("einhundertdreiundzwanzig Euro zweiundzwanzig Cent"));
            Console.WriteLine("einhundertdreiundzwanzigtausend Euro dreiundzwanzig Cent = " + ReverseConvert("einhundertdreiundzwanzigtausend Euro dreiundzwanzig Cent"));
            Console.WriteLine("einhundertdreiundzwanzigmillion Euro fünfzehn Cent = " + ReverseConvert("einhundertdreiundzwanzigmillion Euro fünfzehn Cent"));
            Console.WriteLine("dreitausendein Euro sechs Cent = " + ReverseConvert("dreitausendein Euro sechs Cent"));
            Console.WriteLine("einhundertdreiundzwanzigmilliondreihundert Euro achtundvierzig Cent = " + ReverseConvert("einhundertdreiundzwanzigmilliondreihundert Euro achtundvierzig Cent"));
            Console.WriteLine("einhundertdreiundzwanzigmilliondreihundertachtunddreißigtausendneununddreißig Euro fünfzehn Cent = " + ReverseConvert("einhundertdreiundzwanzigmilliondreihundertachtunddreißigtausendneununddreißig Euro fünfzehn Cent"));
            Console.WriteLine("einhundertdreiundzwanzigtausendfünfhundertvierzehn Euro elf Cent = " + ReverseConvert("einhundertdreiundzwanzigtausendfünfhundertvierzehn Euro elf Cent"));
            Console.WriteLine("vierzehnmillionenachtzehn Euro ein Cent = " + ReverseConvert("vierzehnmillionenachtzehn Euro ein Cent"));
        }

        static string ConvertMoney(decimal amount)
        {
            // EURO
            int euro = (int)Math.Floor(amount);
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
                        if (euro >= 2000000)
                        {
                            euroString += "en"; // million --> millionen
                        }
                    }
                    else if (euroBlocks.Length - i == 2)
                    {
                        euroString += getNumberStringDict[1000];
                    }
                }
                euroString += " Euro";
            }



            // CENT
            int cent = (int)((amount - euro) * 100);
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


        static decimal ReverseConvert(string amount)
        {
            string euroString = "";
            string centString = "";
            decimal result = 0.00m;

            if (amount.Contains("Euro"))
            {
                euroString = amount.Split(new string[] {"Euro"}, StringSplitOptions.None)[0];
                // StringSplitOptions.TrimEntries would have dealt with white-spaces, but that option is only available since .NET 5, and this solution targets the 4.8 framework.
                euroString = euroString.Trim();
                euroString = euroString.Replace("millionen", "million");
                amount = amount.Split(new string[] { "Euro" }, StringSplitOptions.None)[1];
            }
            if (amount.Contains("Cent"))
            {
                centString = amount.Split(new string[] { "Cent" }, StringSplitOptions.None)[0];
                centString = centString.Trim();
            }

            if (euroString.Length > 0)
            {
                result += ReverseConvert(euroString, true);
            }
            if (centString.Length > 0)
            {
                result += ReverseConvert(centString, false);
            }

            return result;
        }

        // Looks at the whole number-word and traverses it number by number, while building a charArray containing the digits corresponding to the number-words.
        static decimal ReverseConvert(string amount, bool isEuro)
        {
            string numberString = "00";
            if (isEuro)
            {
                numberString += "0";
            }
            
            string number;
            string nextNumber;

            if (amount.Contains("million"))
            {
                numberString = "000000000";
            }
            else if (amount.Contains("tausend"))
            {
                numberString = "000000";
            }

            char[] numberChars = numberString.ToCharArray();
            int i = -1; // Used for traversing numberChars
            if (isEuro)
            {
                i++;
            }

            while (amount.Length > 0)
            {
                number = GetNumber(amount);
                if (number.Equals(""))
                {
                    break;
                }
                if (number.Equals("million"))
                {
                    amount = amount.Remove(0, number.Length);
                    if (amount.Contains("tausend") == false) // If there are no thousands in the number, we gotta jump over the thousands block
                    {
                        i += 3;
                    }
                    continue;
                }
                if (number.Equals("tausend"))
                {
                    amount = amount.Remove(0, number.Length);
                    continue;
                }
                nextNumber = GetNextNumber(amount, number.Length);
                if (nextNumber.Equals("und"))
                {
                    string[] switchedNumbers = SwitchNumbers(number, amount, number.Length + 3);
                    numberChars[1 + i] = (char)((getNumberIntDict[switchedNumbers[0]] / 10) + '0');
                    numberChars[2 + i] = (char)((getNumberIntDict[switchedNumbers[1]]) + '0');
                    amount = amount.Remove(0, switchedNumbers[0].Length + switchedNumbers[1].Length + 3);
                    i += 3;
                    continue;
                }
                if (nextNumber.Equals("hundert"))
                {
                    numberChars[0 + i] = (char)((getNumberIntDict[number]) + '0');
                    amount = amount.Remove(0, number.Length + nextNumber.Length);
                    continue;
                }
                else
                {
                    if (getNumberIntDict[number] > 9)
                    {
                        numberChars[1 + i] = (char)((getNumberIntDict[number] / 10) + '0');
                        if (getNumberIntDict[number] < 20)
                        {
                            numberChars[2 + i] = (char)((getNumberIntDict[number] % 10) + '0');
                        }
                    }
                    else
                    {
                        numberChars[2 + i] = (char)((getNumberIntDict[number]) + '0');
                    }
                    amount = amount.Remove(0, number.Length);
                    i += 3;
                }
            }

            if (isEuro)
            {
                return decimal.Parse(new string(numberChars));
            }
            else
            {
                return decimal.Parse(new string(numberChars)) / 100;
            }
        }

        static string GetNumber(string numbers)
        {
            for (int i = 3; i  <= numbers.Length; i++)
            {
                if (getNumberIntDict.ContainsKey(numbers.Substring(0, i)))
                {
                    if (IsZigEdgecase(numbers.Substring(0, i), numbers, i))
                    {
                        return numbers.Substring(0, i + 3);
                    }
                    if (IsTensEdgecase(numbers.Substring(0, i), numbers, i))
                    {
                        return numbers.Substring(0, i + 4);
                    }
                    return numbers.Substring(0, i);
                }
            }
            return "";
        }

        // Gets the next number (or "und") after the current number
        static string GetNextNumber(string numbers, int index)
        {
            if (numbers.Length > index+3 && numbers.Substring(index, 3).Equals("und"))
            {
                return "und";
            }
            for (int i = 3; i <= numbers.Length-index; i++)
            {
                if (getNumberIntDict.ContainsKey(numbers.Substring(index, i)))
                {
                    if (IsZigEdgecase(numbers.Substring(index, i), numbers, index+i))
                    {
                        return numbers.Substring(index, i + 3);
                    }
                    if (IsTensEdgecase(numbers.Substring(index, i), numbers, index + i))
                    {
                        return numbers.Substring(index, i + 4);
                    }
                    return numbers.Substring(index, i);
                }
            }
            return "";
        }

        // Switches numbers connected with "und"
        static string[] SwitchNumbers(string firstNumber, string numbers, int index)
        {
            string nextNumber = GetNextNumber(numbers, index);
            return new string[] {nextNumber, firstNumber};
        }

        // Prevents words like "vierzig" from being interpreted separately as "vier" and an error-producing "zig"
        static bool IsZigEdgecase(string number, string numbers, int index)
        {
            if (number.Equals("drei"))
            {
                if (numbers.Substring(index, 3).Equals("ßig"))
                {
                    return true;
                }
            }
            if (number.Equals("vier") || number.Equals("fünf") || number.Equals("acht") || number.Equals("neun"))
            {
                if (numbers.Substring(index, 3).Equals("zig")){
                    return true;
                }
            }
            return false;
        }

        // Prevents numbers in the tens like "vierzehn" from being interpreted separately as "vier" and "zehn"
        static bool IsTensEdgecase(string number, string numbers, int index)
        {
            if (number.Equals("drei") || number.Equals("vier") || number.Equals("fünf") || number.Equals("acht") || number.Equals("neun"))
            {
                if (numbers.Substring(index, 4).Equals("zehn"))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
