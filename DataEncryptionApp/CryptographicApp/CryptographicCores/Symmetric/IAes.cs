using CryptographicApp.Models;
using System.Security.Cryptography;

namespace CryptographicApp.CryptographicCores.Symmetric;

public interface IAes
{
  void SetPadding(PaddingMode padding);
  void SetKeySize(int keySize);
  AESKey GenerateKey();
  Task EncryptFileAsync(string inputFilePath, string outputFilePath, AESKey aesKey);
  Task DecryptFileAsync(string inputFilePath, string outputFilePath, AESKey aesKey);
}
