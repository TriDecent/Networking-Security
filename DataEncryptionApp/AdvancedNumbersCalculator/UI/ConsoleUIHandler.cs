namespace AdvancedNumbersCalculator.UI;

internal class ConsoleUIHandler : IUIHandler
{
  public void ShowMessage(string message)
    => Console.WriteLine(message);

  public static void ShowMessageWithoutNewLine(string message)
    => Console.Write(message + "\t");

  public string GetInput()
    => Console.ReadLine()!;
}