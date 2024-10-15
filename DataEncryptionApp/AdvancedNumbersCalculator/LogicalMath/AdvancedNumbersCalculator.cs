using System.Numerics;
using DataEncryptionApp.AdvancedNumbersCalculator.Utilities;

namespace DataEncryptionApp.AdvancedNumbersCalculator.LogicalMath;

internal class AdvancedNumbersCalculator : IAdvancedNumbersCalculator
{
  public T GenerateRandomPrimeNumber<T>() where T : INumber<T>
  {
    var random = new Random();
    T number;
    int bitSize = typeof(T) switch
    {
      Type type when type == typeof(byte) => 8,
      Type type when type == typeof(ushort) => 16,
      Type type when type == typeof(ulong) => 64,
      _ => throw new NotSupportedException($"Type {typeof(T)} is not supported.")
    };

    do
    {
      number = GenerateRandomNumber<T>(random, bitSize);
    } while (!IsAPrimeNumber(number));

    return number;
  }

  private static T GenerateRandomNumber<T>(Random random, int bitSize) where T : INumber<T>
  {
    byte[] bytes;
    switch (bitSize)
    {
      case 8:
        bytes = new byte[1]; // 8-bit number
        random.NextBytes(bytes);
        return (T)(object)bytes[0];
      case 16:
        bytes = new byte[2]; // 16-bit number
        random.NextBytes(bytes);
        return (T)(object)BitConverter.ToUInt16(bytes);
      case 64:
        bytes = new byte[8]; // 64-bit number
        random.NextBytes(bytes);
        return (T)(object)BitConverter.ToUInt64(bytes);
      default:
        throw new NotSupportedException($"Bit size {bitSize} is not supported.");
    }
  }

  public IEnumerable<BigInteger> GenerateMersennePrimeNumbers()
  {
    int i = 0;
    while (true)
    {
      var number = BigInteger.Pow(2, i) - 1;

      if (IsAPrimeNumber(number))
      {
        yield return number;
      }

      ++i;
    }
  }

  public IEnumerable<BigInteger> Get10LargestPrimeNumbersUnder10FirstMersennePrimeNumber()
  {
    // var mersennePrimeNumbers = GenerateMersennePrimeNumbers().Take(10);
    // var tenthMersennePrimeNumber = mersennePrimeNumbers.Last();

    // return GeneratePrimeNumbersUpTo(tenthMersennePrimeNumber).Reverse().Take(10);
    var primeNumbersUnder10 = GeneratePrimeNumbersUpTo(BigInteger.Parse("524287")).Reverse().Take(10);
    return primeNumbersUnder10;
  }

  private static IEnumerable<BigInteger> GeneratePrimeNumbersUpTo(BigInteger limit)
  {
    // Sieve of Eratosthenes algorithm
    if (limit < 2)
    {
      yield break;
    }

    var isPrime = new Dictionary<BigInteger, bool>();
    for (BigInteger i = 2; i <= limit; i++)
    {
      isPrime[i] = true;
    }

    for (BigInteger i = 2; i * i <= limit; i++)
    {
      if (isPrime[i])
      {
        for (BigInteger j = i * i; j <= limit; j += i)
        {
          isPrime[j] = false;
        }
      }
    }

    for (BigInteger i = 2; i <= limit; i++)
    {
      if (isPrime[i])
      {
        yield return i;
      }
    }
  }

  public bool IsAPrimeNumber<T>(T number) where T : INumber<T>
  {
    if (number <= T.One) return false;
    if (number == T.CreateChecked(2)) return true;
    if (number % T.CreateChecked(2) == T.Zero) return false;

    var limit = number.Sqrt();
    for (T i = T.CreateChecked(3); i <= limit; i += T.CreateChecked(2))
    {
      if (number % i == T.Zero) return false;
    }
    return true;
  }

  public BigInteger GetGCD(BigInteger firstNumber, BigInteger secondNumber)
    => BigInteger.GreatestCommonDivisor(firstNumber, secondNumber);

  public T ComputeModularExponentiation<T>(T baseNumber, T exponent, T modulus) where T : INumber<T>
  {
    if (modulus == T.Zero)
    {
      throw new ArgumentException("Modulus cannot be zero.");
    }

    T result = T.One;
    baseNumber %= modulus;

    while (exponent > T.Zero)
    {
      if (exponent % T.CreateChecked(2) == T.One)
      {
        result = result * baseNumber % modulus;
      }
      exponent /= T.CreateChecked(2);
      baseNumber = baseNumber * baseNumber % modulus;
    }

    return result;
  }
}
