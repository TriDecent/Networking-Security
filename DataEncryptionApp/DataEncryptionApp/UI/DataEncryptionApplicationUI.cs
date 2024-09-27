namespace DataEncryptionApp.UI;

public interface IDataEncryptionApplicationUI
{
  string ReadUserInput();
  void ShowCipherText(string cipherText);
  void ShowPlainText(string plainText);
  void ShowPlainTextBlocks(IEnumerable<string> plainTextBlocks);
  void DisplayMessage(string message);
}