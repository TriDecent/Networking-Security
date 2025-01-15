using CryptographicApp.Enums;
using CryptographicApp.Utils;
using SHA3.Net;

namespace CryptographicApp.CryptographicCores.HashGenerators;

internal class SHA3_512HashGenerator : IHashGenerator
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
    var hashBytes = Sha3.Sha3512().ComputeHash(bytes);
    return hashBytes.ToHex().Replace("-", string.Empty);
  }

  private static string GenerateHashFromHex(string data)
  {
    var bytes = data.HexToBytes();
    var hashBytes = Sha3.Sha3512().ComputeHash(bytes);
    return hashBytes.ToHex().Replace("-", string.Empty);
  }

  private static string GenerateHashFromFile(string filePath)
  {
    using var fileStream = File.OpenRead(filePath);
    var hashBytes = Sha3.Sha3512().ComputeHash(fileStream);
    return hashBytes.ToHex().Replace("-", string.Empty);
  }

  public string GenerateHash(Stream stream)
  {
    var hashBytes = Sha3.Sha3512().ComputeHash(stream);
    return hashBytes.ToHex().Replace("-", string.Empty);
  }
}
