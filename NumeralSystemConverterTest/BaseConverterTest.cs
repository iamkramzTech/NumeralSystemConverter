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
        [Test]
        public void DecimalToBinaryTest()
        {
            string result = BaseConverter.DecimalToBinary(33);
            Assert.That(result,Is.EqualTo("100001"));
        }
    }
}