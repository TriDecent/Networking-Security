using System.Numerics;
using DataEncryptionApp.AdvancedNumbersCalculator.LogicalMath;
using DataEncryptionApp.AdvancedNumbersCalculator.UI;

namespace DataEncryptionApp.AdvancedNumbersCalculator.App;

internal class AdvancedNumbersCalculatorApp
{
  private readonly IAdvancedNumbersCalculator _calculator;
  private readonly IUIHandler _uiHandler;

  public AdvancedNumbersCalculatorApp(IAdvancedNumbersCalculator calculator, IUIHandler uiHandler)
  {
    _calculator = calculator;
    _uiHandler = uiHandler;
  }

  public void Run()
  {
    // 987654321098765432109812312321321376543210
    // 123456789012345678123123123213901234567890
    _uiHandler.ShowMessage("Welcome to the Advanced Numbers Calculator App!");
    _uiHandler.ShowMessage("Generate a 8 bits random prime number: ");
    _uiHandler.ShowMessage(_calculator.GenerateRandomPrimeNumber<byte>().ToString());

    _uiHandler.ShowMessage("Generate a 16 bits random prime number: ");
    _uiHandler.ShowMessage(_calculator.GenerateRandomPrimeNumber<ushort>().ToString());

    _uiHandler.ShowMessage("Generate a 64 bits random prime number: ");
    _uiHandler.ShowMessage(_calculator.GenerateRandomPrimeNumber<ulong>().ToString());


    _uiHandler.ShowMessage("Generate the first 10 Mersenne prime numbers: ");
    foreach (var number in _calculator.GenerateMersennePrimeNumbers().Take(10))
    {
      _uiHandler.ShowMessage(number.ToString());
    }

    _uiHandler.ShowMessage("Get the 10 largest prime numbers under 10 first Mersenne prime number: ");

    foreach (var number in _calculator.Get10LargestPrimeNumbersUnder10FirstMersennePrimeNumber())
    {
      ConsoleUIHandler.ShowMessageWithoutNewLine(number.ToString());
    }

    _uiHandler.ShowMessage(string.Empty);


    _uiHandler.ShowMessage("The greatest common divisor of 2 large numbers: ");
    _uiHandler.ShowMessage(
      _calculator.GetGCD(
        BigInteger.Parse("987654321098765432109812312321321376543210"), // _uiHandler.GetInput()
        BigInteger.Parse("123456789012345678123123123213901234567890") // _uiHandler.GetInput()
      ).ToString()
    );

    _uiHandler.ShowMessage("Enter base number, exponent, modulus sequentially: ");
    var baseNumber = BigInteger.Parse(_uiHandler.GetInput());
    var exponent = BigInteger.Parse(_uiHandler.GetInput());
    var modulus = BigInteger.Parse(_uiHandler.GetInput());
    _uiHandler.ShowMessage(
      $"Compute the modular exponentiation of {baseNumber}^{exponent} mod {modulus}: ");

    _uiHandler.ShowMessage(_calculator.ComputeModularExponentiation(baseNumber, exponent, modulus).ToString());
  }
}
