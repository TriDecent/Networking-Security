using System.Text.RegularExpressions;
using CryptographicApp.Utils;

namespace CryptographicApp.CryptographicCores.KeysRepository;

public partial class SecureKeyStorage
{
  private const string hyphen = "-----";
  [GeneratedRegex(@"public_key(\d+)\.pem")]
  private static partial Regex RSAKeyNumberPattern();

  [GeneratedRegex(@"aes_key(\d+)\.key")]
  private static partial Regex AESKeyNumberPattern();

  public string ReadSingleRSAKey(string filePath)
    => File.ReadAllText(filePath);

  public void SaveKeyPair(RSAKeyPair keyPair)
  {
    var directory = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath)!, "KeyPairs");
    Directory.CreateDirectory(directory);

    var newNumber = Directory.GetFiles(directory, "public_key*.pem")
      .Select(file => RSAKeyNumberPattern().Match(file))
      .Where(match => match.Success && int.TryParse(match.Groups[1].Value, out _))
      .Select(match => int.Parse(match.Groups[1].Value))
      .DefaultIfEmpty(0)
      .Max() + 1;

    Write(Path.Combine(directory, $"public_key{newNumber}.pem"), keyPair.PublicKey);
    Write(Path.Combine(directory, $"private_key{newNumber}.pem"), keyPair.PrivateKey);
  }

  public void SaveAESKey(AESKey aesKey)
  {
    var directory = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath)!, "AESKeys");
    Directory.CreateDirectory(directory);

    var newNumber = Directory.GetFiles(directory, "aes_key*.key")
      .Select(file => AESKeyNumberPattern().Match(file))
      .Where(match => match.Success && int.TryParse(match.Groups[1].Value, out _))
      .Select(match => int.Parse(match.Groups[1].Value))
      .DefaultIfEmpty(0)
      .Max() + 1;

    string content = $"""
      {hyphen}BEGIN AES KEY{hyphen}
      {aesKey.Key}
      {hyphen}END AES KEY{hyphen}
      {hyphen}BEGIN AES IV{hyphen}
      {aesKey.IV}
      {hyphen}END AES IV{hyphen}
      """;

    Write(Path.Combine(directory, $"aes_key{newNumber}.key"), content);
  }

  public AESKey ReadAESKey(string filePath)
  {
    var content = File.ReadAllText(filePath);
    var key = content
      .Between($"{hyphen}BEGIN AES KEY{hyphen}", $"{hyphen}END AES KEY{hyphen}")
      .Trim();

    var iv = content
      .Between($"{hyphen}BEGIN AES IV{hyphen}", $"{hyphen}END AES IV{hyphen}")
      .Trim();

    return new AESKey(key, iv);
  }

  private static void Write(string filePath, string key)
    => File.WriteAllText(filePath, key);
}
