namespace DataEncryptionApp.DataEncryption;

public interface ICrackingDataEncryption : IDataEncryption
{
  IEnumerable<string> CrackingDecrypt(string cipherText);
}