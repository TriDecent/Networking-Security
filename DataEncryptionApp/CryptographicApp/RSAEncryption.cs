using System.Security.Cryptography;
using System.Text;

namespace CryptographicApp;

public interface IRSAEncryption
{
  (string PublicKey, string PrivateKey) GenerateKey();
  string Encrypt(string data, string publicKeyPem, DataFormat dataFormat);
  string Decrypt(string encryptedData, string privateKeyPem, DataFormat dataFormat);
  void EncryptFile(string inputFile, string outputFile, string publicKeyPem);
  void DecryptFile(string inputFile, string outputFile, string privateKeyPem);
}

public class RSAEncryption(RSA rsa, RSAEncryptionPadding padding) : IRSAEncryption
{
  private readonly Dictionary<RSAEncryptionPadding, int> _paddingOverhead = new()
  {
    [RSAEncryptionPadding.Pkcs1] = 11,
    [RSAEncryptionPadding.OaepSHA1] = 42,
    [RSAEncryptionPadding.OaepSHA256] = 66,
    [RSAEncryptionPadding.OaepSHA384] = 98,
    [RSAEncryptionPadding.OaepSHA512] = 130
  };
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

  private string DecryptFromText(string encryptedText, string privateKeyPem)
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

  public void EncryptFile(string inputFile, string outputFile, string publicKeyPem)
  {
    _rsa.ImportFromPem(publicKeyPem);
    int maxChunkSize = (_rsa.KeySize / 8) - _paddingOverhead[_padding];

    using var inputStream = File.OpenRead(inputFile);
    using var outputStream = File.Create(outputFile);

    var buffer = new byte[maxChunkSize];
    int bytesRead;

    while ((bytesRead = inputStream.Read(buffer, 0, maxChunkSize)) > 0)
    {
      var chunk = buffer[..bytesRead];
      var encryptedChunk = _rsa.Encrypt(chunk, _padding);

      outputStream.Write(BitConverter.GetBytes(encryptedChunk.Length));
      outputStream.Write(encryptedChunk);
    }
  }

  public void DecryptFile(string inputFile, string outputFile, string privateKeyPem)
  {
    _rsa.ImportFromPem(privateKeyPem);
    using var inputStream = new BufferedStream(File.OpenRead(inputFile));
    using var outputStream = new BufferedStream(File.Create(outputFile));

    var buffer = new byte[sizeof(int)];

    while (inputStream.Read(buffer, 0, sizeof(int)) == sizeof(int))
    {
      int chunkSize = BitConverter.ToInt32(buffer);
      var encryptedChunk = new byte[chunkSize];

      if (inputStream.Read(encryptedChunk, 0, chunkSize) == chunkSize)
      {
        var decryptedChunk = _rsa.Decrypt(encryptedChunk, _padding);
        outputStream.Write(decryptedChunk);
      }
    }
  }
}