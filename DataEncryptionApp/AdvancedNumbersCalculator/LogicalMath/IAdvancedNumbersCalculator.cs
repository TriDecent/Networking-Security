using System.Numerics;

namespace AdvancedNumbersCalculator.LogicalMath;

internal interface IAdvancedNumbersCalculator
{
  T GenerateRandomPrimeNumber<T>() where T : INumber<T>;
  IEnumerable<BigInteger> GenerateMersennePrimeNumbers();
  IEnumerable<BigInteger> Get10LargestPrimeNumbersUnder10FirstMersennePrimeNumber();
  bool IsAPrimeNumber<T>(T number) where T : INumber<T>;
  BigInteger GetGCD(BigInteger firstNumber, BigInteger secondNumber);
  T ComputeModularExponentiation<T>(T baseNumber, T exponent, T modulus) where T : INumber<T>;
}
