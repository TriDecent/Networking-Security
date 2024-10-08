using System.Text;

namespace DataEncryptionApp.DataEncryption.ShiftCipher
{
  public class VigenereEncryption : AlphabetShiftEncryption
  {
    private const int AlphabetSize = 26;

    public override string Encrypt(string plainText, string key)
    {
      StringBuilder cipherText = new();
      key = FormatKey(plainText, key);

      for (int i = 0; i < plainText.Length; i++)
      {
        char plainChar = plainText[i];

        if (char.IsLetter(plainChar))
        {
          char offset = char.IsUpper(plainChar) ? 'A' : 'a';
          char keyChar = key[i];

          // Ci = (pi + ki mod m) mod 26
          int cipherChar = (plainChar + keyChar - 2 * offset) % AlphabetSize;
          cipherText.Append((char)(cipherChar + offset));
        }
        else
        {
          cipherText.Append(plainChar);
        }
      }

      return cipherText.ToString();
    }

    public override string Decrypt(string cipherText, string key)
    {
      StringBuilder plainText = new();
      key = FormatKey(cipherText, key);

      for (int i = 0; i < cipherText.Length; i++)
      {
        char cipherChar = cipherText[i];

        if (char.IsLetter(cipherChar))
        {
          char offset = char.IsUpper(cipherChar) ? 'A' : 'a';
          char keyChar = key[i];

          // pi = (Ci - ki + m) mod 26
          int plainChar = (cipherChar - keyChar + AlphabetSize) % AlphabetSize;
          plainText.Append((char)(plainChar + offset));
        }
        else
        {
          plainText.Append(cipherChar);
        }
      }

      return plainText.ToString();
    }

    public override IEnumerable<string> CrackingDecrypt(string cipherText)
    {
      throw new NotImplementedException();
    }

    private string FormatKey(string text, string key)
    {
      StringBuilder extendedKey = new();
      int keyLength = key.Length;

      for (int i = 0; i < text.Length; i++)
      {
        extendedKey.Append(key[i % keyLength]);
      }

      return extendedKey.ToString();
    }
  }
}
