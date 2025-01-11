using System.Text;

namespace CryptographicApp.Utils;

public static class StringExtension
{
  public static string Between(this string str, string first, string last)
  {
    int pos1 = str.IndexOf(first) + first.Length;
    int pos2 = str.IndexOf(last);
    return pos2 > pos1 ? str[pos1..pos2] : string.Empty;
  }

  public static byte[] StringToBytes(this string @string)
    => Encoding.UTF8.GetBytes(@string);

  public static byte[] HexToBytes(this string hex) => Convert.FromHexString(hex);

  public static byte[] Base64ToBytes(this string base64String)
    => Convert.FromBase64String(base64String);
}
