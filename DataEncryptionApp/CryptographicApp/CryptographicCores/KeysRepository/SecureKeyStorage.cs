using CryptographicApp.Models;
using CryptographicApp.Utils;
using System.Text.RegularExpressions;

namespace CryptographicApp.CryptographicCores.KeysRepository;

public partial class SecureKeyStorage
{
  private const string HYPHEN = "-----";

  [GeneratedRegex(@"public_key(\d+)\.pem")]
  private static partial Regex RSAKeyNumberPattern();

  [GeneratedRegex(@"aes_key(\d+)\.key")]
  private static partial Regex AESKeyNumberPattern();

  public const string RSA_KEYS_FOLDER = "RSAKeys";
  public const string AES_KEYS_FOLDER = "AESKeys";

  public string ReadSingleRSAKey(string filePath)
  => File.ReadAllText(filePath);

  public void SaveKeyPair(RSAKey keyPair)
  {
    var directory = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath)!, RSA_KEYS_FOLDER);
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
    var directory = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath)!, AES_KEYS_FOLDER);
    Directory.CreateDirectory(directory);

    var newNumber = Directory.GetFiles(directory, "aes_key*.key")
      .Select(file => AESKeyNumberPattern().Match(file))
      .Where(match => match.Success && int.TryParse(match.Groups[1].Value, out _))
      .Select(match => int.Parse(match.Groups[1].Value))
      .DefaultIfEmpty(0)
      .Max() + 1;

    string content = $"""
      {HYPHEN}BEGIN AES KEY{HYPHEN}
      {aesKey.Key}
      {HYPHEN}END AES KEY{HYPHEN}
      {HYPHEN}BEGIN AES IV{HYPHEN}
      {aesKey.IV}
      {HYPHEN}END AES IV{HYPHEN}
      """;

    Write(Path.Combine(directory, $"aes_key{newNumber}.key"), content);
  }

  public AESKey ReadAESKey(string filePath)
  {
    var content = File.ReadAllText(filePath);
    var key = content
      .Between($"{HYPHEN}BEGIN AES KEY{HYPHEN}", $"{HYPHEN}END AES KEY{HYPHEN}")
      .Trim();

    var iv = content
      .Between($"{HYPHEN}BEGIN AES IV{HYPHEN}", $"{HYPHEN}END AES IV{HYPHEN}")
      .Trim();

    return new AESKey(key, iv);
  }

  private static void Write(string filePath, string key)
    => File.WriteAllText(filePath, key);
}
