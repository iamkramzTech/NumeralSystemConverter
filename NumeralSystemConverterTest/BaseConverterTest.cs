using NUnit.Framework;
using NumeralSystemConverter;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
namespace NumeralSystemConverterTest
{
    /// <summary>
    /// Test class for verifying the functionality of the BaseConverter class methods.
    /// </summary>
    public class BaseConverterTest
    {
        /// <summary>
        /// Sets up the test environment before each test method execution.
        /// </summary>
        [SetUp]
        public void Setup() { }

        #region -- Decimal to Binary Test Method
        /// <summary>
        /// Tests the DecimalToBinary method of the BaseConverter class.
        /// binary length must be 32 bits
        /// </summary>
        /// <param name="decimalNumber">The decimal number to convert.</param>
        /// <param name="expectedBinary">The expected 32 bits binary representation of the decimal number.</param>
        [TestCase(2147483647, "01111111111111111111111111111111")]
        [TestCase(-Int32.MaxValue, "10000000000000000000000000000001")]
        //[TestCase(7, "111")]
        //[TestCase(10, "1010")]
        [TestCase(-9999, "11111111111111111101100011110001")]
        //[TestCase(33, "100001")]
        [TestCase(-2147483648, "10000000000000000000000000000000")]
      //  [TestCase(2147483648, "10000000000000000000000000000000")]
        [TestCase(-2147483638, "10000000000000000000000000001010")]
        [TestCase(2147483638, "01111111111111111111111111110110")]
        [TestCase(255, "00000000000000000000000011111111")]
        [TestCase(-255, "11111111111111111111111100000001")]
        //[TestCase(Int32.MaxValue - 10, "1111111111111111111111111110101")]
        [TestCase(-50, "11111111111111111111111111001110")]
        [TestCase(50, "00000000000000000000000000110010")]
        public void DecimalToBinaryTest(int decimalNumber, string expectedBinary)
        {
            string result = BaseConverter.DecimalToBinary(decimalNumber);
            Assert.That(result, Is.EqualTo(expectedBinary));
        }
        #endregion
        #region -- Negative input DecimalToBinary --
        // <summary>
        /// Tests that the DecimalToBinary method throws ArgumentOutOfRangeException for negative inputs.
        /// </summary>
        //[Test]
        //public void DecimalToBinaryArgumentOutofRangeExceptionThrown()
        //{
        //    var testCases = new[]
        //    {
        //         new TestCaseData(-1, "Input must be a non - negative integer. (Parameter 'decimalNumber')"),
        //         new TestCaseData(-9999, "Input must be a non - negative integer. (Parameter 'decimalNumber')"),
        //         new TestCaseData(Int32.MinValue, "Input must be a non - negative integer. (Parameter 'decimalNumber')")
        //    };
        //    foreach (var testCaseData in testCases)
        //    {
        //        ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(() => BaseConverter.DecimalToBinary((int)testCaseData.Arguments[0]));
        //        Assert.That(exception.Message, Does.Contain(testCaseData.Arguments[1] as string));
        //    }
        //}
        #endregion


        #region -- Binary To Decimal Test Methods
        // <summary>
        /// Tests the BinaryToDecimal method of the BaseConverter class.
        /// </summary>
        /// <param name="binaryNumber">The binary number to convert.</param>
        /// <param name="expectedDecimalEquivalent">The expected decimal equivalent of the binary number.</param>
        [TestCase("1111111111111111111111111111111", 2147483647)]
        //[TestCase("10000000000000000000000000001010", int.MinValue)]
        //[TestCase("100111111111111111111111111111101010111", -90)]
        [TestCase("11111111", 255)]
        [TestCase("11110100001000111111", 999999)]
      //  [TestCase("1101101101010101101010011101010110101101010101101010011101010", 33)]
        [TestCase("10000000000000000000000000000000", -2147483648)]
        [TestCase("11111111111111111111111111001110", -50)] 
        [TestCase("10000000000000000000000000000000", -2147483648)]
     // [TestCase("1010111111111111111111111111111111", 1181116006)] //- overflow
        public void BinaryToDecimalTest(string binaryNumber, int expectedDecimalEquivalent)
        {
            var result = BaseConverter.BinaryToDecimal(binaryNumber);
            Assert.That(result, Is.EqualTo(expectedDecimalEquivalent));
        }

        // <summary>
        /// Tests that the BinaryToDecimal method throws ArgumentOutOfRangeException for negative inputs.
        /// </summary>
        [Test]
        public void BinaryToDecimalFormatExceptionThrown()
        {
            var testCases = new[]
            {
                 new TestCaseData("'_,", "Input must consist of '0' and '1' only (Parameter 'binaryNumber')"),
                 new TestCaseData("%1$1001", "Input must consist of '0' and '1' only (Parameter 'binaryNumber')"),
                 new TestCaseData(" 111", "Input must consist of '0' and '1' only (Parameter 'binaryNumber')"),
                 new TestCaseData("1101 ", "Input must consist of '0' and '1' only (Parameter 'binaryNumber')"),
                 new TestCaseData("1011 1101 ", "Input must consist of '0' and '1' only (Parameter 'binaryNumber')"),
            };

            foreach (var testCase in testCases)
            {
                FormatException exception = Assert.Throws<FormatException>(() => BaseConverter.BinaryToDecimal(testCase.Arguments[0] as string));
                Assert.That(exception.Message, Does.Contain(testCase.Arguments[1] as string));
            }


        }
        #endregion

        #region -- BinaryToDecimalNullOrEmptyInputExceptionThrown
        /// <summary>
        ///  Method to Test Binary to Decimal Input is null or empty
        /// </summary>
        [Test]
        public void BinaryToDecimalNullOrEmptyInputExceptionThrown()
        {
            NullorEmptyInputExceptionThrown(BaseConverter.BinaryToDecimal, "Input must not be null or empty");
        }
        #endregion


        #region --  Binary numbers to decimal OverflowException --
        /// <summary>
        /// This method test for overflow Exception
        /// display expected error messages
        /// </summary>
        [Test]
        public void BinaryToDecimalOverflowExceptionThrown()
        {
            BinaryToDecimalOverflowExceptionTest(BaseConverter.BinaryToDecimal, "Binary Number was either too large or too small.Please try another number to convert.");
        }
        #endregion




        #region -- Generic Delegate Type to Check if string is Null Or Empty
        /// <summary>
        /// Method to thrown argument null or empty exception
        /// </summary>
        /// <param name="function">static int Method with arguments string</param>
        /// <param name="expectedErrorMessage">Expected Error Messages</param>
        public void NullorEmptyInputExceptionThrown(Func<string, int> function, string expectedErrorMessage)
        {
            var testCase = new[]
            {
              new TestCaseData(string.Empty,"Input must not be null or empty"),
              new TestCaseData("","Input must not be null or empty")
            };

            foreach (var testCaseData in testCase)
            {
                ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => function(testCaseData.Arguments[0] as string));
                Assert.That(exception.Message, Does.Contain(testCaseData.Arguments[1] as string));
            }
        }
        #endregion


        #region -- Generic Delegate Type to Check if string value to be converted is too large or too snmall.
        /// <summary>
        /// Method to thrown Overflow exception
        /// </summary>
        /// <param name="function">static Method arguments string and return value of an integer</param>
        /// <param name="expectedErrorMessage">Expected Error Messages</param>
        public void OctalToDecimalOverflowExceptionTest(Func<string, int> function, string expectedErrorMessage)
        {
            var testCase = new[]
            {
                 new TestCaseData("777777777777777777777", "Value was either too large or too small for a UInt32."),
                //Please also Test octal number int.minvalue-10
              new TestCaseData("777777777777777777777", "Value was either too large or too small for a UInt32.")
             };

            foreach (var testCaseData in testCase)
            {
                var exception = Assert.Throws<OverflowException>(() => function(testCaseData.Arguments[0] as string));
                Assert.That(exception.Message, Does.Contain(testCaseData.Arguments[1] as string));
            }
        }
        #endregion


        #region -- Generic Delegate Type to Check if string value to be converted is too large or too snmall.
        /// <summary>
        /// Method to thrown Overflow exception
        /// </summary>
        /// <param name="function">static Method arguments string and return value of an integer</param>
        /// <param name="expectedErrorMessage">Expected Error Messages</param>
        public void BinaryToDecimalOverflowExceptionTest(Func<string, int> function, string expectedErrorMessage)
        {
            var testCase = new[]
            {
               
              new TestCaseData("1111111111111111111111111111111111111111111111111111111111001110","Binary Number was either too large or too small. Please try another number to convert"),
              new TestCaseData("1010111111111111111111111111111111","Binary Number was either too large or too small. Please try another number to convert"),
             // new TestCaseData("11111111111111111111111111001110","Binary Number was either too large or too small. Please try another number to convert")
              new TestCaseData("1010111111111111111111111111111111","Binary Number was either too large or too small. Please try another number to convert")
                
            };


            foreach (var testCaseData in testCase)
            {
                var exception = Assert.Throws<OverflowException>(() => function(testCaseData.Arguments[0] as string));
                Assert.That(exception.Message, Does.Contain(testCaseData.Arguments[1] as string));
            }
        }
        #endregion




        #region -- Decimal to Octal Test method --
        // <summary>
        /// Tests the DecimalToOctal method of the BaseConverter class.
        /// </summary>
        /// <param name="decimalNumber">The base 10 number to convert.</param>
        /// <param name="expectedOctalNumber">The expected decimal equivalent of the octal number.</param>
        [TestCase(0, "0")]
        [TestCase(1073741823, "7777777777")]
        [TestCase(999999999, "7346544777")]
        [TestCase(99999999, "575360377")]
       // [TestCase(9223372036854775807, "777777777777777777777")]
        public void DecimalToOctalTest(int decimalNumber, string expectedOctalNumber)
        {
            var result = BaseConverter.DecimalToOctal(decimalNumber);
            Assert.That(result, Is.EqualTo(expectedOctalNumber));
        }
        #endregion

        #region --- Generic Delegate type Func<T,T> to catch negative input in DecimalTo another base conversion method --

        /// <summary>
        /// Test Method fo Negative input in all conversion function
        /// </summary>
        /// <param name="decimalBaseConversionMethod">Conversion Method like DecimalToBinary()</param>
        /// <param name="message">Expected error Messages</param>
        //public void TestConversionMethodWithNegativeInput(Func<int, string> decimalBaseConversionMethod, string expectedErrorMessage)
        //{
        //    var testCases = new[]
        //    {
        //         new TestCaseData(-9999, "Input must be a non - negative integer. (Parameter 'decimalNumber')"),
        //         new TestCaseData(-1, "Input must be a non - negative integer. (Parameter 'decimalNumber')"),
        //         new TestCaseData(-255, "Input must be a non - negative integer. (Parameter 'decimalNumber')"),
        //         new TestCaseData(Int32.MinValue, "Input must be a non - negative integer. (Parameter 'decimalNumber')")
        //    };

        //    foreach (var testCaseData in testCases)
        //    {
        //        ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(() => decimalBaseConversionMethod((int)testCaseData.Arguments[0]));
        //        Assert.That(exception.Message, Does.Contain(testCaseData.Arguments[1] as string));
        //    }

        //}

        #endregion

        #region -- Test Method Decimal to Binary negative input
        /// <summary>
        /// Method to Test Decimal to Binary Negative Input
        /// </summary>
        //[Test]
        //public void DecimalToBinaryNegativeInputThrowsArgumentOutOfRangeException()
        //{
        //    TestConversionMethodWithNegativeInput(BaseConverter.DecimalToBinary, "Input must be a non - negative integer. (Parameter 'decimalNumber')");
        //}

        #endregion

        #region -- Test Method Decimal to Octal negative input
        /// <summary>
        /// Method to Test Decimal to Octal Negative Input
        /// </summary>
        [Test]
        //public void DecimalToOctalNegativeInputThrowsArgumentOutOfRangeException()
        //{
        //    TestConversionMethodWithNegativeInput(BaseConverter.DecimalToOctal, "Input must be a non - negative integer. (Parameter 'decimalNumber')");
        //}
        #endregion

        #region -- Octal to Decimal Test method --
        /// <summary>
        /// Test Method for Octal to Decimal Conversion
        /// </summary>
        /// <param name="octalNumber">string octalNUmber</param>
        /// <param name="expectedDecimalEquivalent">int decimal equivalent</param>


        [TestCase("0", 0)]
        [TestCase("377", 255)]
        [TestCase("7777777777", 1073741823)]
        [TestCase("17777777777", 2147483647)]
        public void OctalToDecimalTest(string octalNumber, int expectedDecimalEquivalent)
        {
            var result = BaseConverter.OctalToDecimal(octalNumber);
            Assert.That(result, Is.EqualTo(expectedDecimalEquivalent));
        }
        #endregion

        #region -- OctalToDecimalNullOrEmptyInputExceptionThrown
        /// <summary>
        ///  Method to Test Octal to Decimal Input is null or empty
        /// </summary>
        [Test]
        public void OctalToDecimalNullOrEmptyInputExceptionThrown()
        {
            NullorEmptyInputExceptionThrown(BaseConverter.OctalToDecimal, "Input must not be null or empty");
        }
        #endregion

        #region -- Invalid Octal numbers Input --
        /// <summary>
        /// This method test the invalid octal numbers
        /// display expected error messages
        /// </summary>
        [Test]
        public void OctalToDecimalFormatExceptionThrown()
        {
            var testCases = new[]
            {
                new TestCaseData("-1", "Input must consist of 0 - 7 only (Parameter 'octalNumber')"),
                new TestCaseData(" ", "Input must consist of 0 - 7 only (Parameter 'octalNumber')"),
                 new TestCaseData("377 1", "Input must consist of 0 - 7 only (Parameter 'octalNumber')"),
                  new TestCaseData("189", "Input must consist of 0 - 7 only (Parameter 'octalNumber')")
            };

            foreach (var testCaseData in testCases)
            {
                FormatException exception = Assert.Throws<FormatException>(() => BaseConverter.OctalToDecimal(testCaseData.Arguments[0] as string));
                Assert.That(exception.Message, Does.Contain(testCaseData.Arguments[1] as string));
            }

        }
        #endregion


        #region --  Octal numbers to decimal OverflowException --
        /// <summary>
        /// This method test for overflow Exception
        /// display expected error messages
        /// </summary>
        [Test]
        public void OctalToDecimalOverflowExceptionThrown()
        {
            OctalToDecimalOverflowExceptionTest(BaseConverter.OctalToDecimal, "Value was either too large or too small for a UInt32.");
        }
        #endregion

        #region -- Decimal To Hexadecimal Test Method --

        /// <summary>
        ///Test Method for Decimal to Hexadecimal Conversion
        /// </summary>
        /// <param name="decimalNumber">integer decimalNumber</param>
        /// <param name="expectedHexadecimalEquivalent">string expected hexadecimal equivalent</param>

        [TestCase(0, "0")]
        [TestCase(255, "FF")]
        [TestCase(-255, "FFFFFF01")]
        [TestCase(11, "B")]
        [TestCase(Int32.MaxValue, "7FFFFFFF")]
        [TestCase(-Int32.MaxValue, "80000001")]
        [TestCase(Int32.MinValue, "80000000")]
        public void DecimalToHexadecimalTest(int decimalNumber, string expectedHexadecimalEquivalent)
        {
            var result = BaseConverter.DecimalToHexadecimal(decimalNumber);
            Assert.That(result, Is.EqualTo(expectedHexadecimalEquivalent));
        }
        #endregion

        #region -- Hexadecimal To Decimal Test Method --

        /// <summary>
        /// Test Method for Hexadecimal to Decimal Conversion
        /// </summary>
        /// <param name="hexadecimalNumber">Hexadecimal string</param>
        /// <param name="expectedHexadecimalEquivalent">integer expected decimal equivalent</param>

        [TestCase("7FFFFFFF",Int32.MaxValue)]
        [TestCase("80000001",-Int32.MaxValue)]
        [TestCase("80000000", Int32.MinValue)]
        [TestCase("0", 0)]
        public void HexadecimalToDecimalTest(string hexadecimalNumber, int expectedHexadecimalEquivalent)
        {
            var result = BaseConverter.HexadecimalToDecimal(hexadecimalNumber);
            Assert.That(result,Is.EqualTo(expectedHexadecimalEquivalent));
        }
        #endregion


        #region -- Invalid Hexadecimal numbers Input --
        /// <summary>
        /// This method test the invalid hexadecimal numbers
        /// display expected error messages
        /// </summary>
        [Test]
        public void HexadecimalToDecimalFormatExceptionThrown()
        {
            var testCases = new[]
            {
                new TestCaseData("7FFFFFFFZX", "Input must consist of 0-9 and A-F only (Parameter 'hexadecimalNumber')"),
                new TestCaseData(" ", "Input must consist of 0-9 and A-F only (Parameter 'hexadecimalNumber')"),
                 new TestCaseData("1K", "Input must consist of 0-9 and A-F only (Parameter 'hexadecimalNumber')"),
                  new TestCaseData("KLMN", "Input must consist of 0-9 and A-F only (Parameter 'hexadecimalNumber')")
            };

            foreach (var testCaseData in testCases)
            {
                FormatException exception = Assert.Throws<FormatException>(() => BaseConverter.HexadecimalToDecimal(testCaseData.Arguments[0] as string));
                Assert.That(exception.Message, Does.Contain(testCaseData.Arguments[1] as string));
            }

        }
        #endregion
    }
}