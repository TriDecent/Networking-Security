using DataEncryptionApp.DataEncryption;

namespace DataEncryptionApp.DataAccess;

public class DataEncryptionRepository(
  IDataEncryption dataEncryption,
  ICrackingDataEncryption crackingDataEncryption,
  IStringsRepository stringsRepository) : IDataEncryptionRepository
{
  private readonly IDataEncryption _dataEncryption = dataEncryption;
  private readonly ICrackingDataEncryption _crackingDataEncryption = crackingDataEncryption;
  private readonly IStringsRepository _stringsRepository = stringsRepository;

  public IEnumerable<string> ReadFromFile(string filePath)
    => _stringsRepository.Read(filePath);

  public void EncryptToFile(string filePath, IEnumerable<string> plainTextBlocks, string key)
  {
    var encryptedBlocks = plainTextBlocks.Select(block => _dataEncryption.Encrypt(block, key));
    _stringsRepository.Write(filePath, encryptedBlocks);
  }

  public void DecryptToFile(string filePath, IEnumerable<string> cipherTextBlocks, string key)
  {
    var plainTextBlocks = cipherTextBlocks.Select(block => _dataEncryption.Decrypt(block, key));
    _stringsRepository.Write(filePath, plainTextBlocks);
  }

  public void CrackingDecryptToFile(string filePath, IEnumerable<string> cipherTextBlocks)
  {
    var decryptedBlocks = cipherTextBlocks.Select(_crackingDataEncryption.CrackingDecrypt);
    var appendedDecryptedBlocks = new List<string>();

    foreach (var block in decryptedBlocks)
    {
      // Since Write of IStringsRepository overwrites the file, we need to append the blocks
      appendedDecryptedBlocks.AddRange(block);
      appendedDecryptedBlocks.Add(" "); // Separate blocks
    }

    _stringsRepository.Write(filePath, appendedDecryptedBlocks);
  }
}