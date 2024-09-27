namespace DataEncryptionApp.UI;

public interface IUIHandler
{
  void DisplayMessage(string message);
  string GetFromUser();
}