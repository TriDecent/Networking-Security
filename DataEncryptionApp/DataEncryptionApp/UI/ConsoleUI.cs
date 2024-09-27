namespace DataEncryptionApp.UI;

public class ConsoleUI : IUIHandler
{
  public void DisplayMessage(string message)
    => Console.WriteLine(message);

  public string? GetFromUser()
    => Console.ReadLine();
}