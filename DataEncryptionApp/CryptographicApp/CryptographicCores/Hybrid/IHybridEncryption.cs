using CryptographicApp.Models;

namespace CryptographicApp.CryptographicCores.Hybrid;

public interface IHybridEncryption
{
  Task EncryptFileAsync(
    string inputFilePath, string outputFilePath, RSAKey rsaKey, AESKey aesKey);
  Task DecryptFileAsync(
    string inputFilePath, string outputFilePath, RSAKey rsaKey);
}
