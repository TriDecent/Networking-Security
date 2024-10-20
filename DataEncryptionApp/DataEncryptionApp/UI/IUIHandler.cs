namespace DataEncryptionApp.UI;

public interface IUIHandler
{
  void DisplayMessage(string message);
  void DisplayMessageWithoutNewLine(string message);
  string GetFromUser();
}