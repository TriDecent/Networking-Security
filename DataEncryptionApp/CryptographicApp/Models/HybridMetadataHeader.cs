namespace CryptographicApp.Models;

public record HybridMetadataHeader(
  int AESKeyLength, int AesIVLength, int HashLength,
  AESKey EncryptedAesKey, string EncryptedHash);
