using DataEncryptionApp.AdvancedNumbersCalculator.App;
using DataEncryptionApp.AdvancedNumbersCalculator.LogicalMath;
using DataEncryptionApp.AdvancedNumbersCalculator.UI;

// try
// {
  var app = new AdvancedNumbersCalculatorApp(
    new AdvancedNumbersCalculator(), 
    new ConsoleUIHandler()
  );
  app.Run();
// }
// catch (Exception ex)
// {
//   Console.WriteLine(
//     $"Some unexpected error occurred. Please try again. Error message: {ex.Message}");
// }
