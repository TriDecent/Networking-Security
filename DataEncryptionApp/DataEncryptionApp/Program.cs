try
{
  var dataEncryption = new DataEncryptionApplication(
    new CaesarWithWhiteSpace(new CaesarEncryption(5)),
    new TextualStringsRepository(),
    new DataEncryptionUI(new ConsoleUI())
  );

  dataEncryption.Run();
}
catch (Exception ex)
{
  Console.WriteLine($"Some unexpected error occurred. Please try again {ex.Message}.");
}

public class DataEncryptionApplication(
  IDataEncryption dataEncryption,
  IStringsRepository stringsRepository,
  DataEncryptionApplicationUI uiHandler)
{
  private readonly IDataEncryption _dataEncryption = dataEncryption;
  private readonly IStringsRepository _stringsRepository = stringsRepository;
  private readonly DataEncryptionApplicationUI _uiHandler = uiHandler;

  public void Run()
  {
    bool isExit = false;
    _uiHandler.DisplayMessage("Welcome to Data Encryption Application!");


    while (!isExit)
    {
      _uiHandler.DisplayMessage("Do you want to encrypt or decrypt text?");
      _uiHandler.DisplayMessage("1. Encrypt\n2. Decrypt\n3. Exit\n");

      string userChoiceToEncryptOrDecrypt = _uiHandler.ReadUserInput();

      if (userChoiceToEncryptOrDecrypt == "3")
      {
        isExit = true;
        continue;
      }
      else if (userChoiceToEncryptOrDecrypt != "1" && userChoiceToEncryptOrDecrypt != "2")
      {
        _uiHandler.DisplayMessage("Please enter correct choice!");
        continue;
      }

      _uiHandler.DisplayMessage("Do you want to write it by hand or read from a file?");
      _uiHandler.DisplayMessage("1. Write\n2. Read from a file\n");
      string userChoiceToWriteOrRead = _uiHandler.ReadUserInput();

      if (userChoiceToEncryptOrDecrypt == "1")
      {
        if (userChoiceToWriteOrRead == "1")
        {
          _uiHandler.DisplayMessage("Please enter the text you want to encrypt:");
          var text = _uiHandler.ReadUserInput();
          var encryptedMessage = _dataEncryption.Encrypt(text);
          _uiHandler.ShowCipherText($"Encrypted message:\n{encryptedMessage}\n");
        }
        else if (userChoiceToWriteOrRead == "2")
        {
          _uiHandler.DisplayMessage("Please enter the path to the file you want to encrypt:");
          // Implement this
        }
      }
      else if (userChoiceToEncryptOrDecrypt == "2")
      {
        if (userChoiceToWriteOrRead == "1")
        {
          _uiHandler.DisplayMessage("Please enter the text you want to decrypt:");
          var text = _uiHandler.ReadUserInput();
          var plainTextBlocks = _dataEncryption.Decrypt(text);
          _uiHandler.ShowPlainTextBlocks(plainTextBlocks);
        }
        else if (userChoiceToWriteOrRead == "2")
        {
          _uiHandler.DisplayMessage("Please enter the path to the file you want to decrypt:");
          // Implement this
        }
      }
    }
  }
}

public interface DataEncryptionApplicationUI
{
  string ReadUserInput();
  void ShowCipherText(string cipherText);
  void ShowPlainTextBlocks(IEnumerable<string> plainTextBlocks);
  void DisplayMessage(string message);
}


public class DataEncryptionUI(IUIHandler uiHandler) : DataEncryptionApplicationUI
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

  public void ShowPlainTextBlocks(IEnumerable<string> plainTextBlocks)
  {
    foreach (var block in plainTextBlocks)
    {
      _uiHandler.DisplayMessage(block);
    }
  }
}


public interface IUIHandler
{
  void DisplayMessage(string message);
  string GetFromUser();
}

public class ConsoleUI : IUIHandler
{
  public void DisplayMessage(string message)
    => Console.WriteLine(message);

  public string? GetFromUser()
    => Console.ReadLine();
}


public interface IDataEncryption
{
  string Encrypt(string plainText);
  IEnumerable<string> Decrypt(string cipherText);
}
public class CaesarWithWhiteSpace(CaesarEncryption caesarEncryption) : IDataEncryption
{
  private readonly CaesarEncryption _caesarEncryption = caesarEncryption;

  public string Encrypt(string plainText)
  {
    var blockEncryptedMessage = plainText
      .Split(' ')
      .Select(_caesarEncryption.Encrypt);

    return string.Join(" ", blockEncryptedMessage);
  }

  public IEnumerable<string> Decrypt(string cipherText)
    => _caesarEncryption.Decrypt(cipherText);
}

public class CaesarEncryption(int numberOfShifts) : IDataEncryption
{
  private readonly Dictionary<char, int> _alphabetMapping = new()
  {
    ['A'] = 1,
    ['B'] = 2,
    ['C'] = 3,
    ['D'] = 4,
    ['E'] = 5,
    ['F'] = 6,
    ['G'] = 7,
    ['H'] = 8,
    ['I'] = 9,
    ['J'] = 10,
    ['K'] = 11,
    ['L'] = 12,
    ['M'] = 13,
    ['N'] = 14,
    ['O'] = 15,
    ['P'] = 16,
    ['Q'] = 17,
    ['R'] = 18,
    ['S'] = 19,
    ['T'] = 20,
    ['U'] = 21,
    ['V'] = 22,
    ['W'] = 23,
    ['X'] = 24,
    ['Y'] = 25,
    ['Z'] = 26
  };

  private readonly int shiftsKey = numberOfShifts;

  public string Encrypt(string plainText)
  {
    string cipherText = "";

    foreach (var letter in plainText)
    {
      var normalizedLetter = char.ToUpper(letter);

      if (!_alphabetMapping.ContainsKey(normalizedLetter))
      {
        continue;
      }

      int letterNum = _alphabetMapping[normalizedLetter];
      int shiftedLetterNum = (letterNum + shiftsKey) % 26;

      var encryptedChar = _alphabetMapping
        .FirstOrDefault(keyValuePair => keyValuePair.Value == shiftedLetterNum).Key;

      if (char.IsLower(letter))
      {
        cipherText += char.ToLower(encryptedChar);
        continue;
      }

      cipherText += encryptedChar;
    }

    return cipherText;
  }

  public IEnumerable<string> Decrypt(string cipherText)
  {
    var results = new List<string>();

    for (int shift = 1; shift < 26; shift++)
    {
      var blockDecryptedMessage = cipherText
          .Split(' ')
          .Select(block => DecryptWithShift(block, shift));

      results.Add(string.Join(" ", blockDecryptedMessage));
    }

    return results;
  }

  private string DecryptWithShift(string cipherText, int shift)
  {
    var result = "";

    foreach (var letter in cipherText)
    {
      var normalizedLetter = char.ToUpper(letter);
      if (!_alphabetMapping.ContainsKey(normalizedLetter))
      {
        continue;
      }

      int letterNum = _alphabetMapping[normalizedLetter];
      int shiftedLetterNum = (letterNum - shift + 26) % 26;

      if (shiftedLetterNum == 0)
      {
        shiftedLetterNum = 26;
      }

      var decryptedChar = _alphabetMapping
        .FirstOrDefault(keyValuePair => keyValuePair.Value == shiftedLetterNum).Key;

      if (char.IsLower(letter))
      {
        result += char.ToLower(decryptedChar);
        continue;
      }

      result += decryptedChar;
    }

    return result;
  }
}

public interface IStringsRepository
{
  IEnumerable<string> Read(string filePath);
  void Write(string filePath, IEnumerable<string> strings);
}

public abstract class StringsRepository : IStringsRepository
{
  public IEnumerable<string> Read(string filePath)
  {
    if (File.Exists(filePath))
    {
      var fileContents = File.ReadAllText(filePath);
      return ConvertStringToCollection(fileContents);
    }
    return [];
  }
  protected abstract IEnumerable<string> ConvertStringToCollection(string fileContents);

  public void Write(string filePath, IEnumerable<string> strings)
  {
    File.WriteAllText(filePath, ConvertCollectionToString(strings));
  }

  protected abstract string ConvertCollectionToString(IEnumerable<string> strings);
}

public class TextualStringsRepository : StringsRepository
{
  private static readonly string Separator = Environment.NewLine;

  protected override IEnumerable<string> ConvertStringToCollection(string fileContents)
    => fileContents.Split(Separator);

  protected override string ConvertCollectionToString(IEnumerable<string> strings)
    => string.Join(Separator, strings);
}
