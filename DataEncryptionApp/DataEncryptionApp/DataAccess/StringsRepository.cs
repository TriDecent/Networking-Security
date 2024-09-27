namespace DataEncryptionApp.DataAccess;

public abstract class StringsRepository : IStringsRepository
{
  public IEnumerable<string> Read(string filePath)
  {
    if (File.Exists(filePath))
    {
      var fileContents = File.ReadAllText(filePath);
      return ConvertStringToCollection(fileContents);
    }
    return [];
  }
  protected abstract IEnumerable<string> ConvertStringToCollection(string fileContents);

  public void Write(string filePath, IEnumerable<string> strings)
  {
    File.WriteAllText(filePath, ConvertCollectionToString(strings));
  }

  protected abstract string ConvertCollectionToString(IEnumerable<string> strings);
}
