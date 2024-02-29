using NUnit.Framework;
using NumeralSystemConverter;
namespace NumeralSystemConverterTest
{
    public class BaseConverterTest
    {
        [SetUp]
        public void Setup()
        {
        }
        [TestCase(int.MaxValue, "1111111111111111111111111111111")]
        //[TestCase(7,"111")]
        //[TestCase(10, "1010")]
        //[TestCase(33, "100001")]
        //[TestCase(34, "100010")]
        [TestCase(255, "11111111")]
        [TestCase(999999, "11110100001000111111")]
        public void DecimalToBinaryTest(int decimalNumber,string expectedBinary)
        {
            string result = BaseConverter.DecimalToBinary(decimalNumber);
            Assert.That(result,Is.EqualTo(expectedBinary));
        }

        [TestCase("11111111",255)]
        [TestCase("11110100001000111111", 999999)]
        public void BinaryToDecimalTest(string binaryNumber,int expectedDecimalEquivalent)
        {
            var result = BaseConverter.BinaryToDecimal(binaryNumber);
            Assert.That(result, Is.EqualTo(expectedDecimalEquivalent));
        }
    }
}