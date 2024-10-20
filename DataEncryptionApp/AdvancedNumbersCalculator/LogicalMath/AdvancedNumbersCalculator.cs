using System.Numerics;
using AdvancedNumbersCalculator.Utilities;

namespace AdvancedNumbersCalculator.LogicalMath;

public class AdvancedNumbersCalculator : IAdvancedNumbersCalculator
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
      Type type when type == typeof(BigInteger) => 2048,
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
        bytes[0] |= 0x80; // Ensure the highest bit is set to 1
        return (T)(object)bytes[0];
      case 16:
        bytes = new byte[2]; // 16-bit number
        random.NextBytes(bytes);
        bytes[0] |= 0x80; // Ensure the highest bit is set to 1
        return (T)(object)BitConverter.ToUInt16(bytes);
      case 64:
        bytes = new byte[8]; // 64-bit number
        random.NextBytes(bytes);
        bytes[0] |= 0x80; // Ensure the highest bit is set to 1
        return (T)(object)BitConverter.ToUInt64(bytes);
      case 2048:
        bytes = new byte[256]; // 2048-bit number
        random.NextBytes(bytes);
        bytes[0] |= 0x80; // Ensure the highest bit is set to 1
        return (T)(object)new BigInteger(bytes);
      default:
        throw new NotSupportedException($"Bit size {bitSize} is not supported.");
    }
  }

  public IEnumerable<BigInteger> GenerateMersennePrimeNumbers()
  {
    int i = 2;
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
    BigInteger tenthMersennePrimeNumber = GenerateMersennePrimeNumbers().Skip(9).First();

    var primes = new List<BigInteger>();

    foreach (var prime in GeneratePrimeNumbersDownTo(tenthMersennePrimeNumber).Take(10))
    {
      primes.Add(prime);
    }

    primes.Reverse();
    return primes;
  }

  private IEnumerable<BigInteger> GeneratePrimeNumbersDownTo(BigInteger limit)
  {
    if (limit < 2)
    {
      yield break;
    }

    // Start checking from odd numbers
    if (limit % 2 == 0)
    {
      limit--;
    }

    for (BigInteger i = limit; i >= 2; i -= 2)
    {
      if (IsAPrimeNumber(i))
      {
        yield return i;
      }
    }

    yield return 2; // 2 is the smallest prime number
  }

  public bool IsAPrimeNumber<T>(T number) where T : INumber<T>
  {
    if (number <= T.One) return false;
    if (number == T.CreateChecked(2) || number == T.CreateChecked(3)) return true;
    if (number % T.CreateChecked(2) == T.Zero || number % T.CreateChecked(3) == T.Zero) return false;


    if (!MillerRabinTest(BigInteger.Parse(number.ToString()!))) return false;

    // Trial Division (works well for small numbers)
    // var limit = number.Sqrt();
    // for (T i = T.CreateChecked(5); i <= limit; i += T.CreateChecked(6))
    // {
    //   if (number % i == T.Zero || number % (i + T.CreateChecked(2)) == T.Zero) return false;
    // }

    return true;
  }

  // Miller-Rabin Primarily Test
  private static bool MillerRabinTest(BigInteger number, int certainty = 10)
  {
    // Check is not needed since the number is already checked in the IsAPrimeNumber method

    BigInteger d = number - 1;
    int s = 0;
    while (d % 2 == 0)
    {
      d /= 2;
      s++;
    }

    Random rng = new();
    for (int i = 0; i < certainty; i++)
    {
      BigInteger a = NumberExtensions.RandomBigInteger(2, number - 2, rng);
      BigInteger x = BigInteger.ModPow(a, d, number);
      if (x == 1 || x == number - 1) continue;

      bool continueLoop = false;
      for (int r = 0; r < s - 1; r++)
      {
        x = BigInteger.ModPow(x, 2, number);
        if (x == 1) return false;
        if (x == number - 1)
        {
          continueLoop = true;
          break;
        }
      }
      if (!continueLoop) return false;
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

  public BigInteger ComputeModularInverse(BigInteger a, BigInteger m)
  {
    BigInteger m0 = m, t, q;
    BigInteger x0 = 0, x1 = 1;

    if (m == 1)
      return 0;

    // Apply extended Euclid Algorithm
    while (a > 1)
    {
      // q is quotient
      q = a / m;
      t = m;

      // m is remainder now, process same as Euclid's algo
      m = a % m;
      a = t;
      t = x0;

      x0 = x1 - q * x0;
      x1 = t;
    }

    // Make x1 positive
    if (x1 < 0)
      x1 += m0;

    return x1;
  }
}
