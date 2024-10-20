namespace DataEncryptionApp.UI;

public class ConsoleUI : IUIHandler
{
  public void DisplayMessage(string message)
    => Console.WriteLine(message);

  public void DisplayMessageWithoutNewLine(string message)
    => Console.Write(message);

  public string? GetFromUser()
    => Console.ReadLine();
}