# MoneyConverter

An exercise in converting amounts of money in numbers to their corresponding words and vice versa. This was my first brush with C#.

**ConvertMoney()** accepts a decimal in the form of X.xx with up to 9 digits before the dot and returns the corresponding amount of money in Euro and Cent in german words. 19.84 --> neunzehn Euro vierundachtzig Cent

**ReverseConvert()** accepts a string of german number-words up to 999.999.999 which must contain "Euro", "Cent", or both, and returns the corresponding amount of money in digits: neunzehn Euro vierundachtzig Cent --> 19,84
<br />
<br />
<br />
<br />
Note that the reverse conversion works, but is unnecessarily complicated and processes the string from left to right while constructing a charArray containing the resulting number.

I have created a much more elegant solution over here: https://github.com/Gjlead/WordToNumberConverter
