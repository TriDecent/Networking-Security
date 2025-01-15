using CryptographicApp.Enums;
using CryptographicApp.Utils;
using System.Security.Cryptography;

namespace CryptographicApp.CryptographicCores.HashGenerators;

internal class SHA1HashGenerator : IHashGenerator
{
  public string GenerateHash(string data, DataFormat dataFormat)
    => dataFormat switch
    {
      DataFormat.Text => GenerateHashFromText(data),
      DataFormat.Hex => GenerateHashFromHex(data),
      DataFormat.File => GenerateHashFromFile(data),
      _ => throw new ArgumentOutOfRangeException(nameof(dataFormat), dataFormat, null)
    };

  private static string GenerateHashFromText(string data)
  {
    var hashBytes = SHA1.HashData(data.StringToBytes());
    return hashBytes.ToHex().Replace("-", string.Empty);
  }

  private static string GenerateHashFromHex(string data)
  {
    var hashBytes = SHA1.HashData(data.HexToBytes());
    return hashBytes.ToHex().Replace("-", string.Empty);
  }

  private static string GenerateHashFromFile(string filePath)
  {
    using var stream = File.OpenRead(filePath);
    var hashBytes = SHA1.HashData(stream);
    return hashBytes.ToHex().Replace("-", string.Empty);
  }

  public string GenerateHash(Stream stream)
  {
    var hashBytes = SHA1.HashData(stream);
    return hashBytes.ToHex().Replace("-", string.Empty);
  }
}
