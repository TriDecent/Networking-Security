using HashCalculator.Enums;
using HashCalculator.Utils;
using System.Security.Cryptography;
using System.Text;

namespace HashCalculator.HashGenerators;

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
    var inputBytes = Encoding.UTF8.GetBytes(data);
    var hashBytes = MD5.HashData(inputBytes);
    return BitConverter.ToString(hashBytes).Replace("-", "");
  }

  private static string GenerateHashFromHex(string hexString)
  {
    var inputBytes = hexString.HexToBytes();
    var hashBytes = MD5.HashData(inputBytes);
    return BitConverter.ToString(hashBytes).Replace("-", "");
  }

  private static string GenerateHashFromFile(string filePath)
  {
    var inputBytes = File.ReadAllBytes(filePath);
    var hashBytes = MD5.HashData(inputBytes);
    return BitConverter.ToString(hashBytes).Replace("-", "");
  }
}
