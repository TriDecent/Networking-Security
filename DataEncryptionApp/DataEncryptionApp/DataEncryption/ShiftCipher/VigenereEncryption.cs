using System.Text;

namespace DataEncryptionApp.DataEncryption.ShiftCipher
{
  public class VigenereEncryption : AlphabetShiftEncryption
  {
    private const int AlphabetSize = 26;

    public override string Encrypt(string plainText, string key)
    {
      StringBuilder cipherText = new();
      key = key.ToUpper();
      int keyIndex = 0;

      for (int i = 0; i < plainText.Length; i++)
      {
        char plainChar = plainText[i];

        if (char.IsLetter(plainChar))
        {
          char offset = char.IsUpper(plainChar) ? 'A' : 'a';
          char keyChar = key[keyIndex % key.Length];
          keyIndex++;


          int cipherChar = (plainChar - offset + (keyChar - 'A')) % AlphabetSize;
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
      key = key.ToUpper();
      int keyIndex = 0;

      for (int i = 0; i < cipherText.Length; i++)
      {
        char cipherChar = cipherText[i];

        if (char.IsLetter(cipherChar))
        {
          char offset = char.IsUpper(cipherChar) ? 'A' : 'a';
          char keyChar = key[keyIndex % key.Length];
          keyIndex++;


          int plainChar = (cipherChar - offset - (keyChar - 'A') + AlphabetSize) % AlphabetSize;
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
  }
}
