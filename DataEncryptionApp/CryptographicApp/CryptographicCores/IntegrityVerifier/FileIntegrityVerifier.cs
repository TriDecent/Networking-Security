using CryptographicApp.CryptographicCores.Asymmetric;
using CryptographicApp.CryptographicCores.HashGenerators;
using CryptographicApp.CryptographicCores.MetadataHeaderExtractor;
using CryptographicApp.CryptographicCores.Symmetric;
using CryptographicApp.Enums;
using CryptographicApp.Models;

namespace CryptographicApp.CryptographicCores.IntegrityVerifier;

public class FileIntegrityVerifier(
  IHeaderMetadataHandler headerHandler,
  IRSAEncryption rsa,
  IAes aes,
  IHashGenerator hashGenerator) : IFileIntegrityVerifier
{
  private readonly IHeaderMetadataHandler _headerHandler = headerHandler;
  private readonly IRSAEncryption _rsa = rsa;
  private readonly IAes _aes = aes;
  private readonly IHashGenerator _hashGenerator = hashGenerator;

  public async Task<bool> Verify(string filePath, RSAKey rsaKey)
  {
    var headerMetadata = await _headerHandler.GetHeaderMetadata(filePath);

    var originalHash = _rsa.Decrypt(
      headerMetadata.EncryptedHash, rsaKey.PrivateKey, DataFormat.Hex);

    var aesKey = _rsa.Decrypt(
      headerMetadata.EncryptedAesKey.Key, rsaKey.PrivateKey, DataFormat.Text);

    var aesIV = _rsa.Decrypt(
      headerMetadata.EncryptedAesKey.IV, rsaKey.PrivateKey, DataFormat.Text);

    using var fileStream = File.OpenRead(filePath);

    _headerHandler.SkipHeader(fileStream, headerMetadata);

    var tempFilePath = Path.GetTempFileName();
    using var tempStream1 = new FileStream(
      tempFilePath,
      FileMode.Open,
      FileAccess.Write,
      FileShare.ReadWrite | FileShare.Delete
    );

    await fileStream.CopyToAsync(tempStream1);

    // temporary
    await tempStream1.FlushAsync();
    tempStream1.Close();
    // delete this after implementing hash generators using stream
    // temporary

    await _aes.DecryptFileAsync(
      tempFilePath, tempFilePath, new AESKey(aesKey, aesIV));

    var computedHash = _hashGenerator.GenerateHash(tempFilePath, DataFormat.File);

    File.Delete(tempFilePath);

    return originalHash == computedHash;
  }
}
