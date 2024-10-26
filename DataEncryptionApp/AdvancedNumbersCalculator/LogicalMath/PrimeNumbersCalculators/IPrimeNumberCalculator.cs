using System.Numerics;

namespace AdvancedNumbersCalculator.LogicalMath.PrimeNumbersCalculators;

public interface IPrimeNumberCalculator
{
  T GenerateRandomPrimeNumber<T>() where T : INumber<T>;
  IEnumerable<BigInteger> GenerateMersennePrimeNumbers();
  IEnumerable<BigInteger> Get10LargestPrimeNumbersUnder10FirstMersennePrimeNumber();
  bool IsAPrimeNumber<T>(T number) where T : INumber<T>;
}
