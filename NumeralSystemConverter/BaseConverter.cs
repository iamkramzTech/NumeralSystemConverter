using System.Text;

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
        public static int BinaryToDecimal(string binaryNumber)
        {
            return Convert.ToInt32(binaryNumber, 2);
        }
    }
}
