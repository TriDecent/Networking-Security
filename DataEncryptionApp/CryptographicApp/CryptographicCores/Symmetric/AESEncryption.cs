using System.Security.Cryptography;
using CryptographicApp.Models;

namespace CryptographicApp.CryptographicCores.Symmetric;

public interface IAes
{
  AESKey GenerateKey();
}

public class AESEncryption(Aes aes, PaddingMode padding) : IAes
{
  private readonly Aes _aes = aes;
  private readonly PaddingMode _padding = padding;

  public AESKey GenerateKey()
  {
    _aes.GenerateKey();
    _aes.GenerateIV();

    return new AESKey(
      Convert.ToBase64String(_aes.Key), Convert.ToBase64String(_aes.IV));
  }
}
