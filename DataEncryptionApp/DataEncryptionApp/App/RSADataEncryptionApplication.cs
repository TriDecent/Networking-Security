using System.Numerics;
using DataEncryptionApp.DataEncryption.AsymmetricCipher;
using DataEncryptionApp.UI;

namespace DataEncryptionApp.App;

public class RSADataEncryptionApplication : IDataEncryptionApplication
{
  private readonly DataEncryptionApplication _app;
  private readonly RSAEncryption _dataEncryption; // tightly coupled
  private readonly IUIHandler _uiHandler;

  public RSADataEncryptionApplication(
    DataEncryptionApplication app,
    RSAEncryption dataEncryption,
    IUIHandler uIHandler)
  {
    _app = app;
    _dataEncryption = dataEncryption;
    _uiHandler = uIHandler;
  }


  public void Run()
  {
    _uiHandler.DisplayMessage("Enter p, q and e values for RSA key generation!");
    _uiHandler.DisplayMessage("Note: If you don't enter any values, random prime numbers will be generated.");

    _uiHandler.DisplayMessageWithoutNewLine("Enter p: ");
    var pInput = _uiHandler.GetFromUser();
    BigInteger? p = string.IsNullOrWhiteSpace(pInput) ? null : BigInteger.Parse(pInput);

    _uiHandler.DisplayMessageWithoutNewLine("Enter q: ");
    var qInput = _uiHandler.GetFromUser();
    BigInteger? q = string.IsNullOrWhiteSpace(qInput) ? null : BigInteger.Parse(qInput);

    _uiHandler.DisplayMessageWithoutNewLine("Enter e: ");
    var eInput = _uiHandler.GetFromUser();
    int e = string.IsNullOrWhiteSpace(eInput) ? 65537 : int.Parse(eInput);

    _dataEncryption.GenerateKeyPair(p, q, e);
    _app.Run();
  }
}
