using System.Numerics;
using AdvancedNumbersCalculator.LogicalMath.ModularArithmeticCalculators;
using AdvancedNumbersCalculator.LogicalMath.PrimeNumbersCalculators;
using AdvancedNumbersCalculator.UI;

namespace AdvancedNumbersCalculator.App;

internal class AdvancedNumbersCalculatorApp
{
  private readonly IPrimeNumberCalculator _primeNumberCalculator;
  private readonly IModularArithmeticCalculator _modularArithmeticCalculator;
  private readonly IUIHandler _uiHandler;

  public AdvancedNumbersCalculatorApp(
    IPrimeNumberCalculator primeNumberCalculator,
    IModularArithmeticCalculator modularArithmeticCalculator,
    IUIHandler uiHandler)
  {
    _primeNumberCalculator = primeNumberCalculator;
    _modularArithmeticCalculator = modularArithmeticCalculator;
    _uiHandler = uiHandler;
  }

  public void Run()
  {
    ShowWelcomeMessage();
    ShowRandomPrimeNumbers();
    ShowMersennePrimeNumbers();
    ShowTenLargestPrimeNumbersUnderMersenne();
    CheckPrimeNumber();
    ShowGCDOfLargeNumbers();
    ComputeModularExponentiation();
  }

  private void ShowWelcomeMessage()
  {
    _uiHandler.ShowMessage("Welcome to the Advanced Numbers Calculator App!");
  }

  private void ShowRandomPrimeNumbers()
  {
    _uiHandler.ShowMessage("Generate a 8 bits random prime number: ");
    _uiHandler.ShowMessage(_primeNumberCalculator.GenerateRandomPrimeNumber<byte>().ToString());

    _uiHandler.ShowMessage("Generate a 16 bits random prime number: ");
    _uiHandler.ShowMessage(_primeNumberCalculator.GenerateRandomPrimeNumber<ushort>().ToString());

    _uiHandler.ShowMessage("Generate a 64 bits random prime number: ");
    _uiHandler.ShowMessage(_primeNumberCalculator.GenerateRandomPrimeNumber<ulong>().ToString());
  }

  private void ShowMersennePrimeNumbers()
  {
    _uiHandler.ShowMessage("Generate the first 10 Mersenne prime numbers: ");
    foreach (var number in _primeNumberCalculator.GenerateMersennePrimeNumbers().Take(10))
    {
      _uiHandler.ShowMessage(number.ToString());
    }
  }

  private void ShowTenLargestPrimeNumbersUnderMersenne()
  {
    _uiHandler.ShowMessage("Get the 10 largest prime numbers under 10 first Mersenne prime number: ");
    foreach (var number in _primeNumberCalculator.Get10LargestPrimeNumbersUnder10FirstMersennePrimeNumber())
    {
      ConsoleUIHandler.ShowMessageWithoutNewLine(number.ToString());
    }
    _uiHandler.ShowMessage(string.Empty);
  }

  private void CheckPrimeNumber()
  {
    _uiHandler.ShowMessage("Enter a number to check if it is a prime number: ");
    _uiHandler.ShowMessage(
      _primeNumberCalculator.IsAPrimeNumber(
        BigInteger.Parse(_uiHandler.GetInput())
      ).ToString()
    );
  }

  private void ShowGCDOfLargeNumbers()
  {
    _uiHandler.ShowMessage("The greatest common divisor of 2 large numbers: ");
    _uiHandler.ShowMessage(
      _modularArithmeticCalculator.GetGCD(
        BigInteger.Parse(_uiHandler.GetInput()), // 987654321098765432109812312321321376543210
        BigInteger.Parse(_uiHandler.GetInput())  // 123456789012345678901231231232132765432109
      ).ToString()
    );
  }

  private void ComputeModularExponentiation()
  {
    _uiHandler.ShowMessage("Enter base number, exponent, modulus sequentially: ");
    var baseNumber = BigInteger.Parse(_uiHandler.GetInput());
    var exponent = BigInteger.Parse(_uiHandler.GetInput());
    var modulus = BigInteger.Parse(_uiHandler.GetInput());
    _uiHandler.ShowMessage(
      $"The modular exponentiation of {baseNumber}^{exponent} mod {modulus}: ");

    _uiHandler.ShowMessage(_modularArithmeticCalculator.ComputeModularExponentiation(baseNumber, exponent, modulus).ToString());
  }
}
