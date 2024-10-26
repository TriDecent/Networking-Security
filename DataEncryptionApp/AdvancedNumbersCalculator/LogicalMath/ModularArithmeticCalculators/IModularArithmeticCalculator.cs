using System.Numerics;

namespace AdvancedNumbersCalculator.LogicalMath.ModularArithmeticCalculators;

public interface IModularArithmeticCalculator
{
  BigInteger GetGCD(BigInteger firstNumber, BigInteger secondNumber);
  T ComputeModularExponentiation<T>(T baseNumber, T exponent, T modulus) where T : INumber<T>;
  T ComputeModularInverse<T>(T number, T modulus) where T : INumber<T>;
}
