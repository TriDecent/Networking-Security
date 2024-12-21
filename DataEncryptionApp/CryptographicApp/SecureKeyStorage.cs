namespace CryptographicApp;

public static class SecureKeyStorage
{
  public static string Read(string filePath)
    => File.ReadAllText(filePath);

  public static void Write(string filePath, string key)
    => File.WriteAllText(filePath, key);
}