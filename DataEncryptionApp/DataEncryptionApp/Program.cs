using DataEncryptionApp.App;
using DataEncryptionApp.DataAccess;
using DataEncryptionApp.DataEncryption;
using DataEncryptionApp.DataEncryption.AffineCipher;
using DataEncryptionApp.DataEncryption.AsymmetricCipher;
using DataEncryptionApp.DataEncryption.PlayFair;
using DataEncryptionApp.DataEncryption.ShiftCipher;
using DataEncryptionApp.UI;

// Input from console can just be 1 block of text
// Input from file can be multiple blocks of text

try
{
  var uiHandler = new ConsoleUI();
  uiHandler.DisplayMessage("Select encryption method:");
  uiHandler.DisplayMessage("1. Affine Encryption");
  uiHandler.DisplayMessage("2. PlayFair Encryption");
  uiHandler.DisplayMessage("3. Caesar Encryption");
  uiHandler.DisplayMessage("4. Vigenere Encryption");
  uiHandler.DisplayMessage("5: RSA Encryption");
  uiHandler.DisplayMessage("Note: Caesar Encryption will be used for any incorrect input.");
  uiHandler.DisplayMessageWithoutNewLine("Enter choice (1/2/3/4): ");
  var choice = Console.ReadLine();

  ICrackingDataEncryption currentEncryption = choice switch
  {
    "1" => new AffineEncryption(),
    "2" => new PlayFairEncryption(),
    "3" => new CaesarEncryption(),
    "4" => new VigenereEncryption(),
    "5" => new RSAEncryption(),
    _ => new CaesarEncryption()
  };

  var dataEncryption = new DataEncryptionApplication(
    currentEncryption, // default 
    currentEncryption, // cracking
    new DataEncryptionRepository(
      currentEncryption, // default
      currentEncryption, // cracking
      new TextualStringsRepository()
    ),
    new DataEncryptionUI(uiHandler)
  );

  dataEncryption.Run();
}
catch (Exception ex)
{
  Console.WriteLine(
    $"Some unexpected error occurred. Please try again. Error message: {ex.Message}");
}

Console.ReadLine();