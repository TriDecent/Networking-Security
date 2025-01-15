using CryptographicApp.CryptographicCores.Asymmetric;
using CryptographicApp.CryptographicCores.HashGenerators;
using CryptographicApp.CryptographicCores.Hybrid;
using CryptographicApp.CryptographicCores.IntegrityVerifier;
using CryptographicApp.CryptographicCores.MetadataHeaderExtractor;
using CryptographicApp.CryptographicCores.Symmetric;
using System.Security.Cryptography;

namespace CryptographicApp.Utils;

public static class CryptoComponentFactory
{
  public static RSAEncryption CreateRSAEncryption(RSA rsa, RSAEncryptionPadding padding)
    => new(rsa, padding);

  public static AESEncryption CreateAESEncryption(Aes aes)
    => new(aes);

  public static HybridEncryption CreateHybridEncryption(
    RSA rsa,
    Aes aes,
    RSAEncryptionPadding rsaPadding,
    PaddingMode aesPadding,
    Enums.HashAlgorithm hashAlgorithm)
  {
    var rsaEncryption = CreateRSAEncryption(rsa, rsaPadding);
    var headerHandler = CreateHeaderHandler(rsa, rsaPadding, hashAlgorithm);
    var aesEncryption = new AESEncryption(aes);

    aesEncryption.SetPadding(aesPadding);

    return new HybridEncryption(rsaEncryption, aesEncryption, headerHandler);
  }

  public static IHashGenerator CreateHashGenerator(Enums.HashAlgorithm algorithm)
    => algorithm switch
    {
      Enums.HashAlgorithm.MD5 => new MD5HashGenerator(),
      Enums.HashAlgorithm.SHA1 => new SHA1HashGenerator(),
      Enums.HashAlgorithm.SHA3_256 => new SHA3_512HashGenerator(),
      _ => new SHA256HashGenerator()
    };

  public static HeaderMetadataHandler CreateHeaderHandler(
    RSA rsa,
    RSAEncryptionPadding padding,
    Enums.HashAlgorithm algorithm)
  {
    var rsaEncryption = CreateRSAEncryption(rsa, padding);
    var hashGenerator = CreateHashGenerator(algorithm);
    return new HeaderMetadataHandler(hashGenerator, rsaEncryption);
  }

  public static FileIntegrityVerifier CreateIntegrityVerifier(
    RSA rsa,
    Aes aes,
    RSAEncryptionPadding padding,
    Enums.HashAlgorithm algorithm)
  {
    var headerHandler = CreateHeaderHandler(rsa, padding, algorithm);
    var rsaEncryption = new RSAEncryption(rsa, padding);
    var aesEncryption = new AESEncryption(aes);
    var hashGenerator = CreateHashGenerator(algorithm);

    return new FileIntegrityVerifier(
      headerHandler,
      rsaEncryption,
      aesEncryption,
      hashGenerator);
  }
}
