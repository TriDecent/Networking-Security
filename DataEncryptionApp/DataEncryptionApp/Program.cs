using DataEncryptionApp.App;
using DataEncryptionApp.DataAccess;
using DataEncryptionApp.DataEncryption.AffineCipher;
using DataEncryptionApp.DataEncryption.PlayFair;
using DataEncryptionApp.DataEncryption.ShiftCipher;
using DataEncryptionApp.UI;

// Input from console can just be 1 block of text
// Input from file can be multiple blocks of text

try
{
  var currentEncryption = new AffineEncryption(); // Alters here to use another encryption.

  // var playFairEncryption = new PlayFairEncryption("Harry Potter");

  var dataEncryption = new DataEncryptionApplication(
    currentEncryption, // default 
    currentEncryption, // cracking
    new DataEncryptionRepository(
      currentEncryption, // default
      currentEncryption, // cracking
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