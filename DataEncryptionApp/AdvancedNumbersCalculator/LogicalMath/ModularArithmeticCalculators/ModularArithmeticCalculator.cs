using System.Numerics;

namespace AdvancedNumbersCalculator.LogicalMath.ModularArithmeticCalculators;

public class ModularArithmeticCalculator : IModularArithmeticCalculator
{
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

    public T ComputeModularInverse<T>(T number, T modulus) where T : INumber<T>
    {
        T m0 = modulus;
        T t;
        T q;
        T x0 = T.Zero;
        T x1 = T.One;

        if (modulus == T.One)
            return T.Zero;

        // Apply extended Euclid Algorithm
        while (number > T.One)
        {
            // q is quotient
            q = number / modulus;
            t = modulus;

            // m is remainder now, process same as Euclid's algo
            modulus = number % modulus;
            number = t;
            t = x0;

            x0 = x1 - q * x0;
            x1 = t;
        }

        // Make x1 positive
        if (x1 < T.Zero)
            x1 += m0;

        return x1;
    }
}