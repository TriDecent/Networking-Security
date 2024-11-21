namespace HashCalculator;

public static class StringUtils
{
  public static byte[] HexToBytes(this string hex)
  {
    if (string.IsNullOrEmpty(hex))
    {
      throw new InvalidOperationException("Hex string cannot be null or empty");
    }

    if (hex.Length % 2 != 0)
    {
      throw new FormatException("Hex string must have an even length");
    }

    byte[] bytes = new byte[hex.Length / 2];
    for (int i = 0; i < hex.Length; i += 2)
    {
      bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
    }

    return bytes;
  }
}
