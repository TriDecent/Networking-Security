namespace DataEncryptionApp.DataAccess;

public interface IStringsRepository
{
  IEnumerable<string> Read(string filePath);
  void Write(string filePath, IEnumerable<string> strings);
}