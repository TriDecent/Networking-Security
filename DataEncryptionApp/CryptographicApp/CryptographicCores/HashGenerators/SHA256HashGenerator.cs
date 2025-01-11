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
    var hash = SHA256.HashData(bytes);
    return Convert.ToHexString(hash).Replace("-", "");
  }

  private static string GenerateHashFromHex(string data)
  {
    var bytes = data.HexToBytes();
    var hash = SHA256.HashData(bytes);
    return Convert.ToHexString(hash).Replace("-", "");
  }

  private static string GenerateHashFromFile(string filePath)
  {
    var bytes = File.ReadAllBytes(filePath);
    var hash = SHA256.HashData(bytes);
    return Convert.ToHexString(hash).Replace("-", "");
  }
}
