using CryptographicApp.CryptographicCores.Asymmetric;
using CryptographicApp.CryptographicCores.MetadataHeaderExtractor;
using CryptographicApp.CryptographicCores.Symmetric;
using CryptographicApp.Enums;
using CryptographicApp.Models;
using CryptographicApp.Utils;

namespace CryptographicApp.CryptographicCores.Hybrid;

public class HybridEncryption(
  IRSAEncryption rsa, IAes aes,
  IHeaderMetadataHandler headerHandler) : IHybridEncryption
{
  private readonly IRSAEncryption _rsa = rsa;
  private readonly IAes _aes = aes;
  private readonly IHeaderMetadataHandler _headerHandler = headerHandler;

  public async Task EncryptFileAsync(
    string inputFilePath, string outputFilePath, RSAKey rsaKey, AESKey aesKey)
  {
    var headerMetadata = _headerHandler.GenerateHeaderMetadata(
      inputFilePath, rsaKey, aesKey);

    using var outputStream = new FileStream(
      outputFilePath,
      FileMode.Create,
      FileAccess.Write,
      FileShare.ReadWrite
    );

    await _headerHandler.WriteMetadataHeaderTo(outputStream, headerMetadata);

    var tempFilePath = Path.GetTempFileName();

    await _aes.EncryptFileAsync(inputFilePath, tempFilePath, aesKey);

    using var tempStream = File.OpenRead(tempFilePath);
    await tempStream.CopyToAsync(outputStream);

    File.Delete(tempFilePath);
  }

  public async Task DecryptFileAsync(
    string inputFilePath, string outputFilePath, RSAKey rsaKey)
  {
    var headerMetadata = await _headerHandler.GetHeaderMetadata(inputFilePath);
    var aesKey = DecryptAESKeyFromMetadataHeader(headerMetadata, rsaKey);

    using var inputFileStream = File.OpenRead(inputFilePath);

    _headerHandler.SkipHeader(inputFileStream, headerMetadata);

    var tempFilePath = Path.GetTempFileName();
    using var tempFileStream = new FileStream(
      tempFilePath,
      FileMode.Create,
      FileAccess.Write,
      FileShare.ReadWrite | FileShare.Delete
    );

    await inputFileStream.CopyToAsync(tempFileStream);

    await tempFileStream.FlushAsync();
    tempFileStream.Close();

    await _aes.DecryptFileAsync(tempFilePath, outputFilePath, aesKey);

    File.Delete(tempFilePath);
  }

  private AESKey DecryptAESKeyFromMetadataHeader(
    HybridMetadataHeader headerMetadata, RSAKey rsaKey)
  {
    var encryptedAESKey = headerMetadata.EncryptedAesKey;

    var aesKey = _rsa.Decrypt(
      encryptedAESKey.Key, rsaKey.PrivateKey, DataFormat.Text
    );

    var aesIV = _rsa.Decrypt(
      encryptedAESKey.IV, rsaKey.PrivateKey, DataFormat.Text
    );

    return new AESKey(aesKey, aesIV);
  }
}
