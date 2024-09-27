using DataEncryptionApp.App;
using DataEncryptionApp.DataAccess;
using DataEncryptionApp.DataEncryption.ShiftCipher;
using DataEncryptionApp.UI;

// Input from console can just be 1 block of text
// Input from file can be multiple blocks of text

try
{
  var caesarEncryption = new CaesarWithWhiteSpaceWrapper(new CaesarEncryption()); // Alters here to use another encryption.

  // var playFairEncryption = new PlayFairEncryption("Harry Potter");

  var dataEncryption = new DataEncryptionApplication(
    caesarEncryption, // default 
    caesarEncryption, // cracking
    new DataEncryptionRepository(
      caesarEncryption, // default
      caesarEncryption, // cracking
      new TextualStringsRepository()
    ),
    new DataEncryptionUI(new ConsoleUI())
  );

  dataEncryption.Run();
}
catch (Exception ex)
{
  Console.WriteLine(
    $"Some unexpected error occurred. Please try again. Error message: {ex.Message}");
}

Console.ReadLine();
