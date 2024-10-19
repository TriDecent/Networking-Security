using System.Numerics;

namespace DataEncryptionApp.AdvancedNumbersCalculator.Utilities;

public static class NumberExtensions
{
  public static T Sqrt<T>(this T number) where T : INumber<T>
  {
    if (typeof(T) == typeof(BigInteger))
    {
      return (T)(object)BigIntegerSqrt((BigInteger)(object)number);
    }
    return T.CreateChecked(Math.Sqrt(double.CreateChecked(number)));
  }

  private static BigInteger BigIntegerSqrt(BigInteger value)
  {
    if (value == 0) return 0;
    if (value > 0)
    {
      int bitLength = (int)Math.Ceiling(BigInteger.Log(value, 2));
      BigInteger root = BigInteger.One << (bitLength / 2);

      while (!IsSqrt(value, root))
      {
        root += value / root;
        root /= 2;
      }

      return root;
    }
    throw new ArithmeticException("NaN");
  }

  private static bool IsSqrt(BigInteger n, BigInteger root)
  {
    BigInteger lowerBound = root * root;
    BigInteger upperBound = (root + 1) * (root + 1);
    return n >= lowerBound && n < upperBound;
  }

  public static BigInteger RandomBigInteger(BigInteger minValue, BigInteger maxValue, Random rng)
  {
    byte[] bytes = maxValue.ToByteArray();
    BigInteger result;
    do
    {
      rng.NextBytes(bytes);
      bytes[^1] &= 0x7F; // Đảm bảo số dương
      result = new BigInteger(bytes);
    } while (result < minValue || result > maxValue);
    return result;
  }
}
