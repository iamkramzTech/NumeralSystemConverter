using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;

namespace NumeralSystemConverter
{
    /// <summary>
    /// static class for 32 bits signed integer Base Conversion. It can handle two's complement for negative values
    /// This class cannot be inherited and instantiated.
    /// </summary>
    public static class BaseConverter
    {

        #region -- Decimal To Binary static method --

        /// <summary>
        /// Converts a decimal number to its 32bits signed integer to binary equivalent as a string.
        /// </summary>
        /// <param name="decimalNumber">The non-negative integer to convert. Must be a non-negative integer.</param>
        /// <returns>A string representing the binary equivalent of the decimal number.</returns>
        /// <exception cref="OverflowException">Thrown if <paramref name="decimalNumber"/> is outside the range of 32-bits signed integer</exception>
        public static string DecimalToBinary(int decimalNumber)
        {        
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

        #endregion

        #region -- Binary To Decimal static method --
        /// <summary>
        /// Converts a 32 bits length binary to its decimal equivalent.
        /// </summary>
        /// <param name="binaryNumber">A string representing a binary number.</param>
        /// <returns>A string representing the decimal equivalent of the binary number.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="binaryNumber"/> is null or empty.</exception>
        /// <exception cref="FormatException">Thrown if <paramref name="binaryNumber"/> is invalid binary number</exception>
        /// <exception cref="OverflowException">Thrown if <paramref name="binaryNumber"/> is more than 32 bits capacity</exception>
        public static int BinaryToDecimal(string binaryNumber)
        {
         
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
        #endregion
        #region -- Decimal To Octal static method --
        /// <summary>
        /// Converts a 32 bits signed integer to its octal representation
        /// </summary>
        /// <param name="decimalNumber">A base 10 decimal number integer</param>
        /// <returns>Equivalent Octal number</returns>
        /// <exception cref="OverflowException">Thrown if <paramref name="decimalNumber"/> is outside the range of 32-bits signed integer</exception>
        public static string DecimalToOctal(int decimalNumber)
        {
            if (decimalNumber == 0)
            {
                return "00000000000";
            }
            if (CheckInt32NumberOverflow(decimalNumber))
            {
                throw new OverflowException("Value was outside the range of a 32-bit signed integer");
            }

            string binaryString = DecimalToBinary(decimalNumber);
            StringBuilder octalNumberString = new StringBuilder();

            // Process every 3 bits in the binary string
            for (int i = binaryString.Length; i > 0; i -= 3)
            {
                int octalDigit = 0;
                for (int bit = Math.Max(i - 3, 0); bit < i; bit++)
                {
                    octalDigit = (octalDigit << 1) + (binaryString[bit] - '0');
                }
                octalNumberString.Insert(0, octalDigit.ToString());
            }

            return octalNumberString.ToString();

        }
        #endregion

        #region -- Octal To Decimal static method --
        /// <summary>
        /// Convert an Octal number to its Decimal equivalent
        /// </summary>
        /// <param name="octalNumber">A string representing a binary number.</param>
        /// <returns>Equivalent Decimal number</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="octalNumber"/> is null or empty</exception>
        /// <exception cref="FormatException">Thrown if <paramref name="octalNumber"/>is invalid octal number</exception>
        /// <exception cref="OverflowException">Thrown if <paramref name="octalNumber"/> is more than 32 bits capacity</exception>
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
        #endregion
        #region -- Decimal To Hexadecimal static method --
        /// <summary>
        /// Converts a 32 bits signed integer to hexadecimal Base 16 equivalent
        /// </summary>
        /// <param name="decimalNumber">decimal number</param>
        /// <returns>hexadecimal equivalent</returns>
        /// <exception cref="OverflowException">Thrown if <paramref name="decimalNumber"/> is more than 32 bits capacity</exception>
        public static string DecimalToHexadecimal(int decimalNumber)
        {
            if (CheckInt32NumberOverflow(decimalNumber))
            {
                throw new OverflowException("Value was outside the range of a 32-bit signed integer");
            }
            if (decimalNumber==0)
            {
                return "00000000";
            }
            string binaryString = DecimalToBinary(decimalNumber);
            StringBuilder hexNumberString = new StringBuilder();

            // Process every 4 bits in the binary string
            for (int i = binaryString.Length; i > 0; i-=4)
            {
                var hexDigit = 0;
                for (int bit = Math.Max(i - 4, 0); bit < i; bit++)
                {
                    hexDigit = (hexDigit << 1) + (binaryString[bit] - '0');
                }
                hexNumberString.Insert(0, HexDigitToChar(hexDigit));
            }
            return hexNumberString.ToString();
        }
        #endregion

        #region -- Hexadecimal To Decimal static method --
        /// <summary>
        /// Convert 32 bits HexaDecimal Number to Decimal Base10 equivalent
        /// </summary>
        /// <param name="hexadecimalNumber">hexadecimal string</param>
        /// <returns>decimal equivalent</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="hexadecimalNumber"/>input is null or empty</exception>
        /// <exception cref="FormatException">Thrown if <paramref name="hexadecimalNumber"/>is not valid hexadecimal number</exception>
        /// <exception cref="OverflowException">THrown if <paramref name="hexadecimalNumber"/> is more than int32 capacity</exception>
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
        #endregion

        #region -- Check overflow exception --
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
            //returns false if the conditional statement is false
            return false;
        }
        #endregion

        #region -- Method Helper numeric digits into hex character --
        /// <summary>
        /// a helper method sed to convert a numeric digit into its hexadecimal character equivalent.
        /// </summary>
        /// <param name="digit">hexDigits</param>
        /// <returns>equivlent character</returns>
        private static char HexDigitToChar(int digit)
        {
            if (digit >= 0 && digit <= 9) return (char)('0' + digit);
            return (char)('A' + (digit - 10));
        }
        #endregion
    }
}
