using DataEncryptionApp.DataAccess;
using DataEncryptionApp.DataEncryption;
using DataEncryptionApp.UI;

namespace DataEncryptionApp.App;

public class DataEncryptionApplication
{
  private readonly IDataEncryption _dataEncryption;
  private readonly ICrackingDataEncryption _crackingDataEncryption;
  private readonly IDataEncryptionRepository _dataEncryptionRepository;
  private readonly IDataEncryptionApplicationUI _uiHandler;

  public DataEncryptionApplication(
      IDataEncryption dataEncryption,
      ICrackingDataEncryption crackingDataEncryption,
      IDataEncryptionRepository dataEncryptionRepository,
      IDataEncryptionApplicationUI uiHandler)
  {
    _dataEncryption = dataEncryption;
    _crackingDataEncryption = crackingDataEncryption;
    _dataEncryptionRepository = dataEncryptionRepository;
    _uiHandler = uiHandler;
  }

  public void Run()
  {
    // Only change plainTextFilePath and cipherTextFilePath
    // The result of the encryption will be written to the cipherTextFilePath
    // The result of the decryption will be written to the crackedTextFilePath

    string plainTextFilePath = "plainTextForVigenere.txt";
    string cipherTextFilePath = "cipherTextForVigenere.txt";
    string crackedTextFilePath = "crackedText.txt";

    bool isExit = false;
    _uiHandler.DisplayMessage("Welcome to Data Encryption Application!");

    while (!isExit)
    {
      string encryptionChoice = GetEncryptionChoice();
      if (encryptionChoice == "3")
      {
        isExit = true;
        continue;
      }

      string userChoiceToWriteOrRead = GetUserChoiceToWriteOrRead();

      if (encryptionChoice == "1")
      {
        HandleEncryption(userChoiceToWriteOrRead, plainTextFilePath, cipherTextFilePath);
      }
      else if (encryptionChoice == "2")
      {
        HandleDecryption(userChoiceToWriteOrRead, crackedTextFilePath, cipherTextFilePath);
      }
    }
  }

  private string GetEncryptionChoice()
  {
    _uiHandler.DisplayMessage("Do you want to encrypt or decrypt text?");
    _uiHandler.DisplayMessage("1. Encrypt\n2. Decrypt\n3. Exit\n");
    return _uiHandler.ReadUserInput();
  }

  private string GetUserChoiceToWriteOrRead()
  {
    _uiHandler.DisplayMessage("Do you want to write it by hand or read from a file?");
    _uiHandler.DisplayMessage("1. Write\n2. Read from a file\n");
    return _uiHandler.ReadUserInput();
  }

  private void HandleEncryption(string userChoiceToWriteOrRead, string plainTextFilePath, string cipherTextFilePath)
  {
    string userKey = GetUserKey("Enter your key to encrypt: (If you enter any invalid key, your default key is 'A')", "A");

    if (userChoiceToWriteOrRead == "1")
    {
      EncryptText(userKey);
    }
    else if (userChoiceToWriteOrRead == "2")
    {
      EncryptFile(userKey, plainTextFilePath, cipherTextFilePath);
    }
  }

  private void HandleDecryption(string userChoiceToWriteOrRead, string crackedTextFilePath, string cipherTextFilePath)
  {
    bool doesUserHaveAKey = GetUserKeyAvailability();

    if (doesUserHaveAKey)
    {
      string userKey = GetUserKey("Please enter the key:", null!); // Basically, we don't want to have a default key here.
      HandleDecryptionWhenUserHasKey(userChoiceToWriteOrRead, crackedTextFilePath, cipherTextFilePath, userKey);
    }
    else
    {
      HandleDecryptionWhenUserDoesNotHaveKey(userChoiceToWriteOrRead, crackedTextFilePath, cipherTextFilePath);
    }
  }

  private bool GetUserKeyAvailability()
  {
    _uiHandler.DisplayMessage("Do you have a key? 1: Yes, Anything else: No");
    return _uiHandler.ReadUserInput() == "1";
  }

  private string GetUserKey(string promptMessage, string defaultKey)
  {
    _uiHandler.DisplayMessage(promptMessage);
    var userKey = _uiHandler.ReadUserInput();
    return string.IsNullOrEmpty(userKey) ? defaultKey : userKey;
  }

  private void EncryptText(string userKey)
  {
    _uiHandler.DisplayMessage("Please enter the text you want to encrypt:");
    var text = _uiHandler.ReadUserInput();
    var encryptedMessage = _dataEncryption.Encrypt(text, userKey);
    _uiHandler.ShowCipherText($"Encrypted message:\n{encryptedMessage}\n");
  }

  private void EncryptFile(string userKey, string plainTextFilePath, string cipherTextFilePath)
  {
    _uiHandler.DisplayMessage("Please enter the path to the file you want to encrypt:");
    var stringPath = plainTextFilePath; // change this to get from user
    var plainTextBlocks = _dataEncryptionRepository.ReadFromFile(stringPath);
    _dataEncryptionRepository.EncryptToFile(cipherTextFilePath, plainTextBlocks, userKey);
    _uiHandler.DisplayMessage("Encrypted messages have been written to the file.");
  }

  private void HandleDecryptionWhenUserHasKey(string userChoiceToWriteOrRead, string crackedTextFilePath, string cipherTextFilePath, string userKey)
  {
    if (userChoiceToWriteOrRead == "1")
    {
      DecryptText(userKey);
    }
    else if (userChoiceToWriteOrRead == "2")
    {
      DecryptFile(userKey, crackedTextFilePath, cipherTextFilePath);
    }
  }

  private void HandleDecryptionWhenUserDoesNotHaveKey(string userChoiceToWriteOrRead, string crackedTextFilePath, string cipherTextFilePath)
  {
    if (userChoiceToWriteOrRead == "1")
    {
      CrackDecryptText();
    }
    else if (userChoiceToWriteOrRead == "2")
    {
      CrackDecryptFile(crackedTextFilePath, cipherTextFilePath);
    }
  }

  private void DecryptText(string userKey)
  {
    _uiHandler.DisplayMessage("Please enter the text you want to decrypt:");
    var text = _uiHandler.ReadUserInput();
    var plainText = _dataEncryption.Decrypt(text, userKey);
    _uiHandler.ShowPlainText(plainText);
  }

  private void DecryptFile(string userKey, string crackedTextFilePath, string cipherTextFilePath)
  {
    _uiHandler.DisplayMessage("Please enter the path to the file you want to decrypt:");
    var stringPath = cipherTextFilePath; // change this to get from user
    var cipherTextBlocks = _dataEncryptionRepository.ReadFromFile(stringPath);
    _dataEncryptionRepository.DecryptToFile(crackedTextFilePath, cipherTextBlocks, userKey);
    _uiHandler.DisplayMessage("Decrypted messages have been written to the file.");
  }

  private void CrackDecryptText()
  {
    _uiHandler.DisplayMessage("Please enter the text you want to decrypt:");
    var text = _uiHandler.ReadUserInput();
    var plainTextBlocks = _crackingDataEncryption.CrackingDecrypt(text);
    _uiHandler.ShowPlainTextBlocks(plainTextBlocks);
  }

  private void CrackDecryptFile(string crackedTextFilePath, string cipherTextFilePath)
  {
    _uiHandler.DisplayMessage("Please enter the path to the file you want to decrypt:");
    var stringPath = cipherTextFilePath; // change this to get from user
    var cipherTextBlocks = _dataEncryptionRepository.ReadFromFile(stringPath);
    _dataEncryptionRepository.CrackingDecryptToFile(crackedTextFilePath, cipherTextBlocks);
    _uiHandler.DisplayMessage("Decrypted messages have been written to the file.");
  }
}
