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
    using MD5 md5 = MD5.Create();

    var inputBytes = Encoding.UTF8.GetBytes(data);
    var hashBytes = MD5.HashData(inputBytes);
    var stringBuilder = new StringBuilder();
    for (int i = 0; i < hashBytes.Length; i++)
    {
      stringBuilder.Append(hashBytes[i].ToString("X2"));
    }

    return stringBuilder.ToString();
  }

  private static string GenerateHashFromHex(string hexString)
  {
    using MD5 md5 = MD5.Create();

    var inputBytes = hexString.HexToBytes();
    var hashBytes = MD5.HashData(inputBytes);
    var stringBuilder = new StringBuilder();
    for (int i = 0; i < hashBytes.Length; i++)
    {
      stringBuilder.Append(hashBytes[i].ToString("X2"));
    }

    return stringBuilder.ToString();
  }

  private static string GenerateHashFromFile(string filePath)
  {
    using MD5 md5 = MD5.Create();

    var inputBytes = File.ReadAllBytes(filePath);
    var hashBytes = MD5.HashData(inputBytes);
    var stringBuilder = new StringBuilder();
    for (int i = 0; i < hashBytes.Length; i++)
    {
      stringBuilder.Append(hashBytes[i].ToString("X2"));
    }

    return stringBuilder.ToString();
  }
}
