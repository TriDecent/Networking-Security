using System;
using System.Collections.Generic;
using System.Text;
using DataEncryptionApp.DataEncryption;

namespace DataEncryptionApp.DataEncryption.ShiftCipher
{
  public class VigenereEncryption : AlphabetShiftEncryption
  {
    private const int AlphabetSize = 26;

    // Phương thức mã hóa sử dụng thuật toán Vigenère
    public override string Encrypt(string plainText, string key)
    {
      StringBuilder cipherText = new StringBuilder();
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
          cipherText.Append(plainChar); // Giữ nguyên các ký tự không phải chữ cái
        }
      }

      return cipherText.ToString();
    }

    // Phương thức giải mã sử dụng thuật toán Vigenère
    public override string Decrypt(string cipherText, string key)
    {
      StringBuilder plainText = new StringBuilder();
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
          plainText.Append(cipherChar); // Giữ nguyên các ký tự không phải chữ cái
        }
      }

      return plainText.ToString();
    }

    // Phương thức CrackingDecrypt không được sử dụng trong ví dụ này
    public override IEnumerable<string> CrackingDecrypt(string cipherText)
    {
      throw new NotImplementedException("Chức năng phá mã chưa được triển khai.");
    }

    // Hàm trợ giúp để điều chỉnh chiều dài khóa phù hợp với văn bản
    private string FormatKey(string text, string key)
    {
      StringBuilder extendedKey = new StringBuilder();
      int keyLength = key.Length;

      for (int i = 0; i < text.Length; i++)
      {
        extendedKey.Append(key[i % keyLength]);
      }

      return extendedKey.ToString();
    }
  }
}
