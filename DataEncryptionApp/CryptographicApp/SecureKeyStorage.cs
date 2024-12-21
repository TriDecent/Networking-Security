using System.Text.RegularExpressions;

namespace CryptographicApp;

public partial class SecureKeyStorage
{
  [GeneratedRegex(@"public_key(\d+)\.pem")]
  private static partial Regex KeyNumberPattern();

  public string Read(string filePath)
    => File.ReadAllText(filePath);

  public void Write(string filePath, string key)
    => File.WriteAllText(filePath, key);

  public void SaveKeyPair(KeyPair keyPair)
  {
    var directory = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath)!, "KeyPairs");
    Directory.CreateDirectory(directory);

    var newNumber = Directory.GetFiles(directory, "public_key*.pem")
      .Select(file => KeyNumberPattern().Match(file))
      .Where(match => match.Success && int.TryParse(match.Groups[1].Value, out _))
      .Select(match => int.Parse(match.Groups[1].Value))
      .DefaultIfEmpty(0)
      .Max() + 1;

    Write(Path.Combine(directory, $"public_key{newNumber}.pem"), keyPair.PublicKey);
    Write(Path.Combine(directory, $"private_key{newNumber}.pem"), keyPair.PrivateKey);
  }
}