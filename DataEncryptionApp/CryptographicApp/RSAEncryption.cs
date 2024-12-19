using System.Configuration;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CryptographicApp;

public interface IRSAEncryption
{
  (string PublicKey, string PrivateKey) GenerateKey();
  string Encrypt(string data, string publicKeyPem, DataFormat dataFormat);
  string Decrypt(string encryptedData, string privateKeyPem, DataFormat dataFormat);
  byte[] EncryptFromFile(string filePath, string publicKeyPem);
  byte[] DecryptFromFile(string filePath, string privateKeyPem);
}

public class RSAEncryption(RSA rsa, RSAEncryptionPadding padding) : IRSAEncryption
{
  private readonly RSA _rsa = rsa;
  private readonly RSAEncryptionPadding _padding = padding;

  public (string PublicKey, string PrivateKey) GenerateKey()
  {
    var publicKey = _rsa.ExportRSAPublicKeyPem();
    var privateKey = _rsa.ExportRSAPrivateKeyPem();

    return (publicKey, privateKey);
  }

  public string Encrypt(string data, string publicKeyPem, DataFormat dataFormat)
    => dataFormat switch
    {
      DataFormat.Text => EncryptFromText(data, publicKeyPem),
      DataFormat.Hex => EncryptFromHex(data, publicKeyPem),
      _ => throw new ArgumentException("Unsupported data format", nameof(dataFormat)),
    };

  private string EncryptFromText(string text, string publicKeyPem)
  {
    _rsa.ImportFromPem(publicKeyPem);
    var bytes = Encoding.UTF8.GetBytes(text);
    var encryptedBytes = _rsa.Encrypt(bytes, _padding);

    return Convert.ToBase64String(encryptedBytes);
  }

  private string EncryptFromHex(string hex, string publicKeyPem)
  {
    _rsa.ImportFromPem(publicKeyPem);
    var bytes = Convert.FromHexString(hex);
    var encryptedBytes = _rsa.Encrypt(bytes, _padding);

    return Convert.ToHexString(encryptedBytes);
  }

  public string Decrypt(string data, string privateKeyPem, DataFormat dataFormat)
    => dataFormat switch
    {
      DataFormat.Text => DecryptFromText(data, privateKeyPem),
      DataFormat.Hex => DecryptFromHex(data, privateKeyPem),
      _ => throw new ArgumentException("Unsupported data format", nameof(dataFormat)),
    };

  public string DecryptFromText(string encryptedText, string privateKeyPem)
  {
    _rsa.ImportFromPem(privateKeyPem);
    var bytes = Convert.FromBase64String(encryptedText);
    var decryptedBytes = _rsa.Decrypt(bytes, _padding);
    var text = Encoding.UTF8.GetString(decryptedBytes);

    return text;
  }

  private string DecryptFromHex(string encryptedHex, string privateKeyPem)
  {
     _rsa.ImportFromPem(privateKeyPem);
    var bytes = Convert.FromHexString(encryptedHex);
    var decryptedBytes = _rsa.Decrypt(bytes, _padding);
    var hex = Convert.ToHexString(decryptedBytes);

    return hex;
  }

  public byte[] DecryptFromFile(string filePath, string privateKeyPem)
  {
    throw new NotImplementedException();
  }

  public byte[] EncryptFromFile(string filePath, string publicKeyPem)
  {
    throw new NotImplementedException();
  }
}
