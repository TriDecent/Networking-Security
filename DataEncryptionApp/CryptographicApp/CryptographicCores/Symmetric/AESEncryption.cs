using CryptographicApp.Models;
using CryptographicApp.Utils;
using System.Security.Cryptography;

namespace CryptographicApp.CryptographicCores.Symmetric;

public class AESEncryption(Aes aes) : IAes
{
  private readonly Aes _aes = aes;

  public void SetPadding(PaddingMode padding) => _aes.Padding = padding;

  public void SetKeySize(int keySize) => _aes.KeySize = keySize;

  public AESKey GenerateKey()
  {
    _aes.GenerateKey();
    _aes.GenerateIV();

    return new AESKey(_aes.Key.ToBase64(), _aes.IV.ToBase64());
  }

  public async Task EncryptFileAsync(
    string inputFilePath, string outputFilePath, AESKey aesKey)
  {
    var keyBytes = aesKey.Key.Base64ToBytes();
    var ivBytes = aesKey.IV.Base64ToBytes();

    using var encryptor = _aes.CreateEncryptor(keyBytes, ivBytes);
    using var inputStream = File.OpenRead(inputFilePath);
    using var outputStream = File.Create(outputFilePath);
    using var cryptoStream = new CryptoStream(
      outputStream,
      encryptor,
      CryptoStreamMode.Write
    );

    await inputStream.CopyToAsync(cryptoStream);
    await cryptoStream.FlushFinalBlockAsync();
  }

  public async Task DecryptFileAsync(
    string inputFilePath, string outputFilePath, AESKey aesKey)
  {
    var keyBytes = aesKey.Key.Base64ToBytes();
    var ivBytes = aesKey.IV.Base64ToBytes();

    using var decryptor = _aes.CreateDecryptor(keyBytes, ivBytes);
    using var inputStream = File.OpenRead(inputFilePath);
    using var outputStream = File.Create(outputFilePath);
    using var cryptoStream = new CryptoStream(
      inputStream,
      decryptor,
      CryptoStreamMode.Read
    );

    await cryptoStream.CopyToAsync(outputStream);
  }
}

// public async Task EncryptFileAsync(
//   string inputFilePath, string outputFilePath, AESKey aesKey)
// {
//   var bytes = await File.ReadAllBytesAsync(inputFilePath);
//   var keyBytes = aesKey.Key.Base64ToBytes();
//   var ivBytes = aesKey.IV.Base64ToBytes();

//   using var encryptor = _aes.CreateEncryptor(keyBytes, ivBytes);
//   var encryptedBytes = encryptor.TransformFinalBlock(bytes, 0, bytes.Length);

//   await File.WriteAllBytesAsync(outputFilePath, encryptedBytes);
// }

// public async Task DecryptFileAsync(
//   string inputFilePath, string outputFilePath, AESKey aesKey)
// {
//   var bytes = await File.ReadAllBytesAsync(inputFilePath);
//   var keyBytes = aesKey.Key.Base64ToBytes();
//   var ivBytes = aesKey.IV.Base64ToBytes();

//   using var decryptor = _aes.CreateDecryptor(keyBytes, ivBytes);
//   var decryptedBytes = decryptor.TransformFinalBlock(bytes, 0, bytes.Length);

//   await File.WriteAllBytesAsync(outputFilePath, decryptedBytes);
// }
