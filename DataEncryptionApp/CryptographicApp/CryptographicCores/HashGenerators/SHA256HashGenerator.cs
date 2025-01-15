using CryptographicApp.Enums;
using CryptographicApp.Utils;
using System.Security.Cryptography;

namespace CryptographicApp.CryptographicCores.HashGenerators;

internal class SHA256HashGenerator : IHashGenerator
{
  public string GenerateHash(string data, DataFormat dataFormat)
    => dataFormat switch
    {
      DataFormat.Text => GenerateHashFromText(data),
      DataFormat.Hex => GenerateHashFromHex(data),
      DataFormat.File => GenerateHashFromFile(data),
      _ => throw new NotSupportedException($"Data format {dataFormat} is not supported."),
    };

  private static string GenerateHashFromText(string data)
  {
    var bytes = data.StringToBytes();
    var hashBytes = SHA256.HashData(bytes);
    return hashBytes.ToHex().Replace("-", string.Empty);
  }

  private static string GenerateHashFromHex(string data)
  {
    var bytes = data.HexToBytes();
    var hashBytes = SHA256.HashData(bytes);
    return hashBytes.ToHex().Replace("-", string.Empty);
  }

  private static string GenerateHashFromFile(string filePath)
  {
    using var fileStream = File.OpenRead(filePath);
    var hashBytes = SHA256.HashData(fileStream);
    return hashBytes.ToHex().Replace("-", string.Empty);
  }

  public string GenerateHash(Stream stream)
  {
    var hashBytes = SHA256.HashData(stream);
    return hashBytes.ToHex().Replace("-", string.Empty);
  }
}
