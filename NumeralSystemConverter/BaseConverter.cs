namespace NumeralSystemConverter
{
    public static class BaseConverter
    {
        public static string DecimalToBinary(int decimalNumber)
        {
            return Convert.ToString(decimalNumber, 2);
        }
    }
}
