using AdvancedNumbersCalculator.App;
using AdvancedNumbersCalculator.UI;

try
{
  var app = new AdvancedNumbersCalculatorApp(
    new AdvancedNumbersCalculator.LogicalMath.AdvancedNumbersCalculator(), 
    new ConsoleUIHandler()
  );
  app.Run();
}
catch (Exception ex)
{
  Console.WriteLine(
    $"Some unexpected error occurred. Please try again. Error message: {ex.Message}");
}
