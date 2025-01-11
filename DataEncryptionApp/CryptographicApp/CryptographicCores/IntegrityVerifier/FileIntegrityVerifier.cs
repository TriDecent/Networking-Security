using CryptographicApp.CryptographicCores.Asymmetric;
using CryptographicApp.CryptographicCores.HashGenerators;
using CryptographicApp.CryptographicCores.MetadataHeaderExtractor;
using CryptographicApp.Enums;
using CryptographicApp.Models;

namespace CryptographicApp.CryptographicCores.IntegrityVerifier;

public class FileIntegrityVerifier(
  IHeaderMetadataHandler headerHandler,
  IRSAEncryption rsa,
  IHashGenerator hashGenerator) : IFileIntegrityVerifier
{
  private readonly IHeaderMetadataHandler _headerHandler = headerHandler;
  private readonly IRSAEncryption _rsa = rsa;
  private readonly IHashGenerator _hashGenerator = hashGenerator;

  public async Task<bool> Verify(string filePath, RSAKey rsaKey)
  {
    var headerMetadata = await _headerHandler.GetHeaderMetadata(filePath);

    var originalHash = _rsa.Decrypt(
      headerMetadata.EncryptedHash, rsaKey.PrivateKey, DataFormat.Text);

    using var fileStream = File.OpenRead(filePath);

    _headerHandler.SkipHeader(fileStream, headerMetadata);

    var tempFilePath = Path.GetTempFileName();
    using var tempStream = new FileStream(
      tempFilePath,
      FileMode.Open,
      FileAccess.Write,
      FileShare.ReadWrite | FileShare.Delete
    );

    await fileStream.CopyToAsync(tempStream);

    var computedHash = _hashGenerator.GenerateHash(tempFilePath, DataFormat.File);

    File.Delete(tempFilePath);

    if (originalHash == computedHash) return true;

    return false;
  }
}
