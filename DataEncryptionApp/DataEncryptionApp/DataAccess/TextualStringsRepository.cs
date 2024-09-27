namespace DataEncryptionApp.DataAccess;

public class TextualStringsRepository : StringsRepository
{
  private static readonly string Separator = Environment.NewLine;

  protected override IEnumerable<string> ConvertStringToCollection(string fileContents)
    => fileContents.Split(Separator);

  protected override string ConvertCollectionToString(IEnumerable<string> strings)
    => string.Join(Separator, strings);
}