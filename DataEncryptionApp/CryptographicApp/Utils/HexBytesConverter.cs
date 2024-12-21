namespace CryptographicApp.Utils;

public static class HexUtils
{
  public static byte[] HexToBytes(this string hex) => Convert.FromHexString(hex);

  public static string BytesToHex(this byte[] bytes) => Convert.ToHexString(bytes);

  public static bool IsHexString(this string hex)
  {
    if (string.IsNullOrEmpty(hex))
      return false;

    if (hex.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
      hex = hex[2..];

    if (hex.Length % 2 != 0)
      return false;

    return hex.All(Uri.IsHexDigit);
  }
}