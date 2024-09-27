namespace DataEncryptionApp.DataEncryption;

public interface IDataEncryption
{
  string Encrypt(string plainText, string key);
  string Decrypt(string cipherText, string key);
}