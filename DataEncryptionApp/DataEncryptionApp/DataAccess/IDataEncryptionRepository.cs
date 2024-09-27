namespace DataEncryptionApp.DataAccess;

public interface IDataEncryptionRepository
{
  IEnumerable<string> ReadFromFile(string filePath);
  void EncryptToFile(string filePath, IEnumerable<string> plainTextBlocks, string key);
  void DecryptToFile(string filePath, IEnumerable<string> cipherTextBlocks, string key);
  void CrackingDecryptToFile(string filePath, IEnumerable<string> cipherTextBlocks);
}