namespace DataEncryptionApp.UI;

public class DataEncryptionUI(IUIHandler uiHandler) : IDataEncryptionApplicationUI
{
  private readonly IUIHandler _uiHandler = uiHandler;

  public void DisplayMessage(string message)
  => _uiHandler.DisplayMessage(message);

  public string ReadUserInput()
  {
    var fromUser = _uiHandler.GetFromUser();

    while (fromUser is null || fromUser.Length == 0)
    {
      _uiHandler.DisplayMessage("Please enter correct text!");
      fromUser = _uiHandler.GetFromUser();
    }

    return fromUser;
  }

  public void ShowCipherText(string cipherText)
    => _uiHandler.DisplayMessage(cipherText);

  public void ShowPlainText(string plainText)
    => _uiHandler.DisplayMessage(plainText);

  public void ShowPlainTextBlocks(IEnumerable<string> plainTextBlocks)
  {
    foreach (var block in plainTextBlocks)
    {
      _uiHandler.DisplayMessage(block);
    }
  }
}