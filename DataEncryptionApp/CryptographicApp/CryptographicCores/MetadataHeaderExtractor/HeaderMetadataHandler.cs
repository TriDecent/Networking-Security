using CryptographicApp.CryptographicCores.Asymmetric;
using CryptographicApp.CryptographicCores.HashGenerators;
using CryptographicApp.Enums;
using CryptographicApp.Models;
using CryptographicApp.Utils;
using System.Buffers.Binary;

namespace CryptographicApp.CryptographicCores.MetadataHeaderExtractor;

public class HeaderMetadataHandler(
  IHashGenerator hashGenerator, IRSAEncryption rsa) : IHeaderMetadataHandler
{
  private const int LENGTH_PREFIX_SIZE = sizeof(int);
  private readonly IHashGenerator _hashGenerator = hashGenerator;
  private readonly IRSAEncryption _rsa = rsa;

  public HybridMetadataHeader GenerateHeaderMetadata(
    string filePath, RSAKey rsaKey, AESKey aesKey)
  {
    var inputFileHash = _hashGenerator.GenerateHash(filePath, DataFormat.File);

    var encryptedAESKey = _rsa
      .Encrypt(aesKey.Key, rsaKey.PublicKey, DataFormat.Text);

    var encryptedAesIV = _rsa
      .Encrypt(aesKey.IV, rsaKey.PublicKey, DataFormat.Text);

    var encryptedHash = _rsa
      .Encrypt(inputFileHash, rsaKey.PublicKey, DataFormat.Hex);

    var encryptedAESKeyBytes = encryptedAESKey.Base64ToBytes();
    var encryptedAesIVBytes = encryptedAesIV.Base64ToBytes();
    var encryptedHashBytes = encryptedHash.HexToBytes();

    return new HybridMetadataHeader(
      AESKeyLength: encryptedAESKeyBytes.Length,
      AesIVLength: encryptedAesIVBytes.Length,
      HashLength: encryptedHashBytes.Length,
      EncryptedAesKey: new AESKey(encryptedAESKey, encryptedAesIV),
      EncryptedHash: encryptedHash
    );
  }

  public async Task<HybridMetadataHeader> GetHeaderMetadata(string filePath)
  {
    using var fileStream = File.OpenRead(filePath);

    var buffer = new byte[LENGTH_PREFIX_SIZE];

    await fileStream.ReadExactlyAsync(buffer.AsMemory());
    var aesKeyLength = BinaryPrimitives.ReadInt32BigEndian(buffer);

    await fileStream.ReadExactlyAsync(buffer.AsMemory());
    var aesIVLength = BinaryPrimitives.ReadInt32BigEndian(buffer);

    await fileStream.ReadExactlyAsync(buffer.AsMemory());
    var hashLength = BinaryPrimitives.ReadInt32BigEndian(buffer);

    var aesKeyBytes = new byte[aesKeyLength];
    var aesIVBytes = new byte[aesIVLength];
    var hashBytes = new byte[hashLength];

    await fileStream.ReadExactlyAsync(aesKeyBytes.AsMemory());
    await fileStream.ReadExactlyAsync(aesIVBytes.AsMemory());
    await fileStream.ReadExactlyAsync(hashBytes.AsMemory());

    return new HybridMetadataHeader(
        AESKeyLength: aesKeyLength,
        AesIVLength: aesIVLength,
        HashLength: hashLength,
        EncryptedAesKey: new AESKey(aesKeyBytes.ToBase64(), aesIVBytes.ToBase64()),
        EncryptedHash: hashBytes.ToHex()
    );
  }

  public void SkipHeader(FileStream inputFileStream, HybridMetadataHeader headerMetadata)
  {
    var totalHeaderSize = (LENGTH_PREFIX_SIZE * 3)
      + headerMetadata.AESKeyLength
      + headerMetadata.AesIVLength
      + headerMetadata.HashLength;

    inputFileStream.Seek(totalHeaderSize, SeekOrigin.Current);
  }

  public async Task WriteMetadataHeaderTo(
    Stream targetStream, HybridMetadataHeader metadataHeader)
  {
    var buffer = new byte[LENGTH_PREFIX_SIZE];

    BinaryPrimitives.WriteInt32BigEndian(buffer, metadataHeader.AESKeyLength);
    await targetStream.WriteAsync(buffer.AsMemory());

    BinaryPrimitives.WriteInt32BigEndian(buffer, metadataHeader.AesIVLength);
    await targetStream.WriteAsync(buffer.AsMemory());

    BinaryPrimitives.WriteInt32BigEndian(buffer, metadataHeader.HashLength);
    await targetStream.WriteAsync(buffer.AsMemory());

    await targetStream.WriteAsync(metadataHeader.EncryptedAesKey.Key.Base64ToBytes().AsMemory());

    await targetStream.WriteAsync(metadataHeader.EncryptedAesKey.IV.Base64ToBytes().AsMemory());

    await targetStream.WriteAsync(metadataHeader.EncryptedHash.HexToBytes().AsMemory());
  }
}
