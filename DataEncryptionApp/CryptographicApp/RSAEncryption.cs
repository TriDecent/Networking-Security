using System.Security.Cryptography;
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

  public byte[] EncryptFromFile(string filePath, string publicKeyPem)
  {
    _rsa.ImportFromPem(publicKeyPem);
    var fileBytes = File.ReadAllBytes(filePath);

    int overhead = _paddingOverhead[_padding];
    int maxChunkSize = (_rsa.KeySize / 8) - overhead;
    var encryptedChunks = new List<byte>();

    for (int i = 0; i < fileBytes.Length; i += maxChunkSize)
    {
      var chunk = fileBytes.Skip(i).Take(maxChunkSize).ToArray();
      var encryptedChunk = _rsa.Encrypt(chunk, _padding);
      encryptedChunks.AddRange(encryptedChunk);
    }

    return [.. encryptedChunks];
  }

  public byte[] DecryptFromFile(string filePath, string privateKeyPem)
  {
    _rsa.ImportFromPem(privateKeyPem);
    var encryptedBytes = File.ReadAllBytes(filePath);

    int blockSize = _rsa.KeySize / 8;
    var decryptedChunks = new List<byte>();

    for (int i = 0; i < encryptedBytes.Length; i += blockSize)
    {
      var chunk = encryptedBytes.Skip(i).Take(blockSize).ToArray();
      var decryptedChunk = _rsa.Decrypt(chunk, _padding);
      decryptedChunks.AddRange(decryptedChunk);
    }

    return [.. decryptedChunks];
  }
}