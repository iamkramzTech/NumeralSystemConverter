using NUnit.Framework;
using NumeralSystemConverter;
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

        /// <summary>
        /// Tests the DecimalToBinary method of the BaseConverter class.
        /// </summary>
        /// <param name="decimalNumber">The decimal number to convert.</param>
        /// <param name="expectedBinary">The expected binary representation of the decimal number.</param>
        [TestCase(Int32.MaxValue, "1111111111111111111111111111111")]
        //[TestCase(7, "111")]
        //[TestCase(10, "1010")]
        //[TestCase(33, "100001")]
        //[TestCase(34, "100010")]
        //[TestCase(255, "11111111")]
        [TestCase(Int32.MaxValue - 10, "1111111111111111111111111110101")]
        [TestCase(999999, "11110100001000111111")]
        public void DecimalToBinaryTest(int decimalNumber, string expectedBinary)
        {
            string result = BaseConverter.DecimalToBinary(decimalNumber);
            Assert.That(result, Is.EqualTo(expectedBinary));
        }

        // <summary>
        /// Tests that the DecimalToBinary method throws ArgumentOutOfRangeException for negative inputs.
        /// </summary>
        [Test]
        public void DecimalToBinaryArgumentOutofRangeExceptionThrown()
        {
            var testCases = new[]
            {
                 new TestCaseData(-1, "Input must be a non - negative integer. (Parameter 'decimalNumber')"),
                 new TestCaseData(-9999, "Input must be a non - negative integer. (Parameter 'decimalNumber')"),
                 new TestCaseData(Int32.MinValue, "Input must be a non - negative integer. (Parameter 'decimalNumber')")
            };
            foreach (var testCaseData in testCases)
            {
                ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(() => BaseConverter.DecimalToBinary((int)testCaseData.Arguments[0]));
                Assert.That(exception.Message, Does.Contain(testCaseData.Arguments[1] as string));
            }
        }

        // <summary>
        /// Tests the BinaryToDecimal method of the BaseConverter class.
        /// </summary>
        /// <param name="binaryNumber">The binary number to convert.</param>
        /// <param name="expectedDecimalEquivalent">The expected decimal equivalent of the binary number.</param>
        [TestCase("1111111111111111111111111111111", int.MaxValue)]
        [TestCase("11111111", 255)]
        [TestCase("11110100001000111111", 999999)]
        [TestCase("100001", 33)]
        public void BinaryToDecimalTest(string binaryNumber, int expectedDecimalEquivalent)
        {
            var result = BaseConverter.BinaryToDecimal(binaryNumber);
            Assert.That(result, Is.EqualTo(expectedDecimalEquivalent));
        }

        // <summary>
        /// Tests that the BinaryToDecimal method throws ArgumentOutOfRangeException for negative inputs.
        /// </summary>
        [Test]
        public void BinaryToDecimalArgumentOutofRangeExceptionThrown()
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
                ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(() => BaseConverter.BinaryToDecimal(testCase.Arguments[0] as string));
                Assert.That(exception.Message, Does.Contain(testCase.Arguments[1] as string));
            }


        }

        /// <summary>
        /// Tests that the BinaryToDecimal throws ArgumentNullException
        /// </summary>
        /// <param name="binaryNumber">The binary number to convert</param>
        /// <param name="expectedErrorMessage">Expected ArgumentNullException Error Messages</param>
        [TestCase("", "Input must not be null or empty")]
        public void BinaryToDecimalNullOrEmptyInputExceptionThrown(string binaryNumber, string expectedErrorMessage)
        {

            Assert.Throws<ArgumentNullException>(() => BaseConverter.BinaryToDecimal(binaryNumber), expectedErrorMessage);
        }
    }
}