namespace DataEncryptionApp.DataEncryption.ShiftCipher;
public abstract class AlphabetShiftEncryption : ICrackingDataEncryption
{
  public abstract IEnumerable<string> CrackingDecrypt(string cipherText);
  public abstract string Decrypt(string cipherText, string key);

  public abstract string Encrypt(string plainText, string key);

  protected int ParseStringKeyToInt(string key)
  {
    return !int.TryParse(key, out int shiftKey)
      ? throw new ArgumentException("Key must be a string number that can be parsed to int.")
      : shiftKey;
  }
}
