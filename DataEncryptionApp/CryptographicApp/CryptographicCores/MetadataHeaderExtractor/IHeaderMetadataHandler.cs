using CryptographicApp.Models;

namespace CryptographicApp.CryptographicCores.MetadataHeaderExtractor;

public interface IHeaderMetadataHandler
{
  HybridMetadataHeader GenerateHeaderMetadata(string filePath, RSAKey rsaKey, AESKey aesKey);
  Task<HybridMetadataHeader> GetHeaderMetadata(string filePath);
  void SkipHeader(FileStream fileStream, HybridMetadataHeader headerMetadata);
  Task WriteMetadataHeaderTo(Stream targetStream, HybridMetadataHeader headerMetadata);
}
