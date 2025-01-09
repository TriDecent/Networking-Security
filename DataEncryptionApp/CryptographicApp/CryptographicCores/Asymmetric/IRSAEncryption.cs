using CryptographicApp.Enums;
using CryptographicApp.Models;

namespace CryptographicApp.CryptographicCores.Asymmetric;

public interface IRSAEncryption
{
  void SetKeySize(int keySize);
  RSAKey GenerateKey();
  string Encrypt(string data, string publicKeyPem, DataFormat dataFormat);
  string Decrypt(string encryptedData, string privateKeyPem, DataFormat dataFormat);
  void EncryptFile(string inputFile, string outputFile, string publicKeyPem);
  void DecryptFile(string inputFile, string outputFile, string privateKeyPem);
}
