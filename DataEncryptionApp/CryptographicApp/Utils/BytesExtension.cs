using System.Text;

namespace CryptographicApp.Utils;

public static class BytesExtension
{
  public static string ToString(this byte[] bytes)
    => Encoding.UTF8.GetString(bytes);

  public static string ToBase64(this byte[] bytes)
    => Convert.ToBase64String(bytes);

  public static string ToHex(this byte[] bytes) => Convert.ToHexString(bytes);
}
