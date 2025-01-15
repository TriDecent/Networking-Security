using CryptographicApp.Enums;
using CryptographicApp.Utils;
using System.Security.Cryptography;

namespace CryptographicApp.CryptographicCores.HashGenerators;

public class MD5HashGenerator : IHashGenerator
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
    var inputBytes = data.StringToBytes();
    var hashBytes = MD5.HashData(inputBytes);
    return hashBytes.ToHex().Replace("-", string.Empty);
  }

  private static string GenerateHashFromHex(string data)
  {
    var inputBytes = data.HexToBytes();
    var hashBytes = MD5.HashData(inputBytes);
    return hashBytes.ToHex().Replace("-", string.Empty);
  }

  private static string GenerateHashFromFile(string filePath)
  {
    using var inputStream = File.OpenRead(filePath);
    var hashBytes = MD5.HashData(inputStream);
    return hashBytes.ToHex().Replace("-", string.Empty);
  }

  public string GenerateHash(Stream stream)
  {
    var hashBytes = MD5.HashData(stream);

    return hashBytes.ToHex().Replace("", string.Empty);
  }
}
