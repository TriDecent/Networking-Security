using HashCalculator.Enums;
using HashCalculator.Utils;
using System.Security.Cryptography;
using System.Text;

namespace HashCalculator.HashGenerators;

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
    var hash = SHA1.HashData(Encoding.UTF8.GetBytes(data));
    return BitConverter.ToString(hash).Replace("-", string.Empty);
  }

  private static string GenerateHashFromHex(string data)
  {
    var hash = SHA1.HashData(data.HexToBytes());
    return BitConverter.ToString(hash).Replace("-", string.Empty);
  }

  private static string GenerateHashFromFile(string filePath)
  {
    using var stream = File.OpenRead(filePath);
    var hash = SHA1.HashData(stream);
    return BitConverter.ToString(hash).Replace("-", string.Empty);
  }
}