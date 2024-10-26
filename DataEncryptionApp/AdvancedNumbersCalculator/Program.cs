using AdvancedNumbersCalculator.App;
using AdvancedNumbersCalculator.LogicalMath.PrimeNumbersCalculators;
using AdvancedNumbersCalculator.LogicalMath.ModularArithmeticCalculators;
using AdvancedNumbersCalculator.UI;

try
{
  var app = new AdvancedNumbersCalculatorApp(
    new PrimeNumberCalculator(),
    new ModularArithmeticCalculator(), 
    new ConsoleUIHandler()
  );
  app.Run();
}
catch (Exception ex)
{
  Console.WriteLine(
    $"Some unexpected error occurred. Please try again. Error message: {ex.Message}");
}
