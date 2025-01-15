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

    var tempFilePath1 = Path.GetTempFileName();
    var tempFilePath2 = Path.GetTempFileName();
    using var tempStream = new FileStream(
      tempFilePath1,
      FileMode.Open,
      FileAccess.Write,
      FileShare.ReadWrite | FileShare.Delete
    );

    await fileStream.CopyToAsync(tempStream);

    await tempStream.FlushAsync();
    tempStream.Close();

    await _aes.DecryptFileAsync(
      tempFilePath1, tempFilePath2, new AESKey(aesKey, aesIV));

    var computedHash = _hashGenerator.GenerateHash(tempFilePath2, DataFormat.File);

    File.Delete(tempFilePath1);
    File.Delete(tempFilePath2);

    return originalHash == computedHash;
  }
}
