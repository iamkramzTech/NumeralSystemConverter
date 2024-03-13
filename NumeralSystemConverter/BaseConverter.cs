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
        /// Converts a decimal number to its 32bits binary equivalent as a string.
        /// </summary>
        /// <param name="decimalNumber">The non-negative integer to convert. Must be a non-negative integer.</param>
        /// <returns>A string representing the binary equivalent of the decimal number.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="decimalNumber"/> is negative.</exception>
        public static string DecimalToBinary(int decimalNumber)
        {
            //if (decimalNumber < 0)
            //{
            //    throw new ArgumentOutOfRangeException(nameof(decimalNumber), "Input must be a non - negative integer.");
            //}
            if (decimalNumber == 0)
            {
                return new string('0', 32); // Return 32 zeros for 0
            }
            if(decimalNumber==Int32.MinValue)
            {
                return "10000000000000000000000000000000"; // Special case for int.MinValue
            }

            if(CheckInt32NumberOverflow(decimalNumber))
            {
                throw new OverflowException("Value was outside the range of a 32-bit signed integer");
            }
            var binaryNumber = new int[32];
            StringBuilder binaryNumberString = new StringBuilder();
            int counter = 0;
            // Add one to the inverted binary
            int carry = 1;
            var isNegative = decimalNumber < 0;

            if (isNegative)
            {
                decimalNumber = -decimalNumber; // Make the number positive for bitwise operations
            }

            // Convert the (now positive) decimalNumber to binary
            while (decimalNumber > 0)
            {
                //I forgot to increment the counter so Test Failed because result
                //and expected result doesn't match.
                binaryNumber[counter++] = decimalNumber % 2;
                decimalNumber /= 2;

            }

            // Fill the rest of the binaryNum array with 0s if it's a positive number,
            // since the significant bits have been set and the rest should be 0 for positive numbers.
            while (counter < 32)
            {
                binaryNumber[counter++] = 0;
            }

            if (isNegative)
            {
                // Invert digits
                for (int index = 0; index < 32; index++)
                {
                    binaryNumber[index] = (binaryNumber[index] == 0) ? 1 : 0;
                }

                

                for (int i = 0; i < 32; i++)
                {
                    int sum = binaryNumber[i] + carry;
                    binaryNumber[i] = sum % 2;
                    carry = sum / 2;

                    if (carry == 0) break; // If there's no carry, no need to continue
                }

            }
            // Append binary digits to stringBuilder, constructing the binary string
            for (int index = 31; index >= 0; index--)
            {
                binaryNumberString.Append(binaryNumber[index]);
            }
            ///convert 32 bits binaryNumber To String
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

            /*
             ! Issue Fixed
            [TestCase("100111111111111111111111111111101010111", -90)] this is not correct test must fail
            Handle the numbers outside 32 bit to not be converted value is too large
            */

            // Check if the binary number is more than 32 bits
            if (binaryNumber.Length > 32)
            {
                throw new OverflowException("Binary Number was either too large or too small. Please try another number to convert.");
            }

            // check if input is null or empty
            if (string.IsNullOrEmpty(binaryNumber))
            {
                throw new ArgumentNullException(nameof(binaryNumber), "Input must not be null or empty");
            }

            // Regular Expression to Check if the input is a valid binary number
            if (!Regex.IsMatch(binaryNumber, @"^[01]+$"))
            {
                // throw new ArgumentOutOfRangeException(nameof(binaryNumber), "Input must consist of '0' and '1' only");
                throw new FormatException("Input must consist of '0' and '1' only (Parameter 'binaryNumber')");
            }


            var decimalNumberEquivalent = 0;
            var binaryLength = binaryNumber.Length;
            var digit = 0;
            // Iterate over each digit in the binary number.
            for (int iterator = 0; iterator < binaryLength; iterator++)
            {
                // Convert the digit from char to int and calculate its contribution to the decimal number
                digit = binaryNumber[binaryLength - iterator - 1] - '0';
                // '<<' means bitwise left shift  operator
                // It's a common and efficient technique for powers of 2,
                // which is frequently used in binary manipulation.
                decimalNumberEquivalent = decimalNumberEquivalent + digit * (1 << iterator);

                //can also use Math.Pow instead of bitwise left shift operator
                // decimalNumberEquivalent = decimalNumberEquivalent + digit * (int)(Math.Pow(2,iterator));
            }
            return decimalNumberEquivalent;




        }

        /// <summary>
        /// Converts a decimal number to its octal representation
        /// </summary>
        /// <param name="decimalNumber">A base 10 decimal number integer</param>
        /// <returns>EQuivalent Octal number</returns>
        ///  <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="decimalNumber"/> is out of range.
        public static string DecimalToOctal(int decimalNumber)
        {

            //if (decimalNumber < 0)
            //{
            //    throw new ArgumentOutOfRangeException(nameof(decimalNumber), "Input must be a non - negative integer.");
            //}
            if (CheckInt32NumberOverflow(decimalNumber))
            {
                throw new OverflowException("Value was outside the range of a 32-bit signed integer");
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

        /// <summary>
        /// Convert an Octal number to its Decimal equivalent
        /// </summary>
        /// <param name="octalNumber">A string representing a binary number.</param>
        /// <returns>Equivalent Decimal number</returns>
        /// <exception cref="ArgumentNullException">throws when argument is null or empty</exception>
        /// <exception cref="FormatException">throws when argument is invalid octal number</exception>
        /// /// <exception cref="FormatException">throws when argument is large or too small for integer datatype capacity</exception>
        public static int OctalToDecimal(string octalNumber)
        {

            /*
             * Need to Handle Overflow exception
             */
            // check if input is null or empty
            if (string.IsNullOrEmpty(octalNumber))
            {
                throw new ArgumentNullException(nameof(octalNumber), "Input must not be null or empty");
            }
            // Regular Expression to Check if the input is a valid octal number
            if (!Regex.IsMatch(octalNumber, @"^[0-7]+$"))
            {
                // throw new ArgumentOutOfRangeException(nameof(octalNumber), "Input must consist of 0 - 7 only");
                throw new FormatException("Input must consist of 0 - 7 only (Parameter 'octalNumber')");

            }
            ////built-in function to Convert octal number
            ////to its decimal base 10 representation
            var decimalValue = Convert.ToInt32(octalNumber, 8);

            //if (decimalValue > Int32.MaxValue || decimalValue<Int32.MinValue)
            //{
            //    throw new OverflowException("Value was either too large or too small. Please try another number to convert.");
            //}
            if (CheckInt32NumberOverflow(decimalValue))
            {
                throw new OverflowException("Value was either too large or too small. Please try another number to convert.");
            }
            return decimalValue;


        }

        /// <summary>
        /// Convert Decimal Number to hexadecimal Base 16 equivalent
        /// </summary>
        /// <param name="decimalNumber"></param>
        /// <returns></returns>
        /// <exception cref="OverflowException"></exception>
        public static string DecimalToHexadecimal(int decimalNumber)
        {
            if (CheckInt32NumberOverflow(decimalNumber))
            {
                throw new OverflowException("Value was outside the range of a 32-bit signed integer");
            }
            if (decimalNumber==0)
            {
                return "0";
            }
            var hexaDecNumber = Convert.ToString(decimalNumber, 16);

            return hexaDecNumber.ToUpper();
        }

        /// <summary>
        /// Convert HexaDecimal Number to Decimal Base10 equivalent
        /// </summary>
        /// <param name="hexadecimalNumber"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="OverflowException"></exception>
        public static int HexadecimalToDecimal(string hexadecimalNumber)
        {
            // check if input is null or empty
            if (string.IsNullOrEmpty(hexadecimalNumber))
            {
                throw new ArgumentNullException(nameof(hexadecimalNumber), "Input must not be null or empty");
            }

            // Regular Expression to Check if the input is a valid hexadecimal number
            if (!Regex.IsMatch(hexadecimalNumber, @"^[0-9A-Fa-f]+$"))
            {
                
                throw new FormatException("Input must consist of 0-9 and A-F only (Parameter 'hexadecimalNumber')");

            }

            var decimalValue = Convert.ToInt32(hexadecimalNumber, 16);

            if (CheckInt32NumberOverflow(decimalValue))
            {
                throw new OverflowException("Value was either too large or too small. Please try another number to convert.");
            }
           

            return decimalValue;
            
        }

        /// <summary>
        /// Check if integer value is less than minimum capacity of a 32 bits integer or greater than maximum capacity of 32 bits integer
        /// </summary>
        /// <param name="value">integer number to be check</param>
        /// <returns>returns boolean value</returns>
        private static bool CheckInt32NumberOverflow(int value)
        {
            if(value < Int32.MinValue || value > Int32.MaxValue)
            {
                return true;
            }

            return false;
        }


    }
}
