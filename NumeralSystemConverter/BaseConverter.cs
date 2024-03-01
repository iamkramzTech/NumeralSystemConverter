using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;

namespace NumeralSystemConverter
{
    /// <summary>
    /// A static class for Base Number Conversion
    /// static class means cannot be instantiated to create an object
    /// </summary>
    public static class BaseConverter
    {

        /// <summary>
        /// Converts a non-negative decimal number to its binary equivalent as a string.
        /// </summary>
        /// <param name="decimalNumber">The non-negative integer to convert. Must be a non-negative integer.</param>
        /// <returns>A string representing the binary equivalent of the decimal number.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="decimalNumber"/> is negative.</exception>
        public static string DecimalToBinary(int decimalNumber)
        {
            if (decimalNumber < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(decimalNumber), "Input must be a non - negative integer.");
            }
            if(decimalNumber==0)
            {
                return "0";
            }
            var binaryNumber = new int[32];
            StringBuilder binaryNumberString = new StringBuilder();
            int counter = 0;

            while(decimalNumber>0)
            {
                binaryNumber[counter] = decimalNumber % 2;
                decimalNumber /= 2;
                counter += 1; //increment counter
            }

            // Construct the binary string in reverse order
            for (var index = counter-1; index>=0; index--)
            {
                binaryNumberString.Append(binaryNumber[index]);
            }
            return binaryNumberString.ToString();
        }

        /// <summary>
        /// Converts a binary number to its decimal equivalent.
        /// </summary>
        /// <param name="binary">A string representing a binary number.</param>
        /// <returns>A string representing the decimal equivalent of the binary number.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="binaryNumber"/> is null or empty.
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="binaryNumber"/> has characters other than 1 and 0.
        public static int BinaryToDecimal(string binaryNumber)
        {
            // check if input is null or empty
            if (string.IsNullOrEmpty(binaryNumber))
            {
                throw new ArgumentNullException(nameof(binaryNumber), "Input must not be null or empty");
            }

            // Regular Expression to Check if the input is a valid binary number
            if (!Regex.IsMatch(binaryNumber, @"^[01]+$"))
            {
                throw new ArgumentOutOfRangeException(nameof(binaryNumber), "Input must consist of '0' and '1' only");
            }


            var decimalNumberEquivalent = 0;
            var binaryLength = binaryNumber.Length;
            var digit = 0;
            // Iterate over each digit in the binary number.
            for (int iterator = 0; iterator < binaryLength; iterator++)
            {
                // Convert the digit from char to int and calculate its contribution to the decimal number
                digit = binaryNumber[binaryLength- iterator-1] - '0';
                // '<<' means bitwise left shift  operator
                // It's a common and efficient technique for powers of 2,
                // which is frequently used in binary manipulation.
                decimalNumberEquivalent = decimalNumberEquivalent + digit * (1<<iterator);

                //can also use Math.Pow instead of bitwise left shift operator
               // decimalNumberEquivalent = decimalNumberEquivalent + digit * (int)(Math.Pow(2,iterator));
            }
            return decimalNumberEquivalent;
        }

        public static string DecimalToOctal(int decimalNumber)
        {

            if (decimalNumber < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(decimalNumber), "Input must be a non - negative integer.");
            }
            if (decimalNumber == 0)
            {
                return "0";
            }

            var octalNumber = new int[32];
            StringBuilder octalNumberString = new StringBuilder();
            int counter = 0;

            while (decimalNumber > 0)
            {
                octalNumber[counter] = decimalNumber % 8;
                decimalNumber /= 8;
                counter += 1; //increment counter
            }

            // Construct the binary string in reverse order
            for (var index = counter - 1; index >= 0; index--)
            {
                octalNumberString.Append(octalNumber[index]);
            }
            return octalNumberString.ToString();

            ////built-in function to Convert decimal number
            ////to its octal representation
            //return Convert.ToString(decimalNumber, 8);
        }
    }
}
