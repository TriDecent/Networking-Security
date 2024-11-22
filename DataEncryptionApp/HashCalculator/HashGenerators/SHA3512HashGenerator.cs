using HashCalculator.Enums;
using HashCalculator.Utils;
using SHA3.Net;

namespace HashCalculator.HashGenerators;

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
    var hash = Sha3.Sha3512().ComputeHash(System.Text.Encoding.UTF8.GetBytes(data));
    return BitConverter.ToString(hash).Replace("-", string.Empty);
  }

  private static string GenerateHashFromHex(string data)
  {
    var hash = Sha3.Sha3512().ComputeHash(data.HexToBytes());
    return BitConverter.ToString(hash).Replace("-", string.Empty);
  }

  private static string GenerateHashFromFile(string data)
  {
    var bytes = File.ReadAllBytes(data);
    var hash = Sha3.Sha3512().ComputeHash(bytes);
    return BitConverter.ToString(hash).Replace("-", string.Empty);
  }
}
