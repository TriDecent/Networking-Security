using System;
using System.Collections.Generic;
using System.Text;

namespace DataEncryptionApp.DataEncryption.ShiftCipher
{
  public class VigenereEncryption : AlphabetShiftEncryption
  {
    private const int AlphabetSize = 26;

    public override string Encrypt(string plainText, string key)
    {
      StringBuilder cipherText = new();
      key = key.ToUpper();  // Đảm bảo key là in hoa để thống nhất xử lý
      int keyIndex = 0;     // Sử dụng chỉ số riêng cho key

      for (int i = 0; i < plainText.Length; i++)
      {
        char plainChar = plainText[i];

        if (char.IsLetter(plainChar))
        {
          char offset = char.IsUpper(plainChar) ? 'A' : 'a';
          char keyChar = key[keyIndex % key.Length]; // Chỉ số key tuần hoàn
          keyIndex++; // Chỉ tăng khi gặp ký tự là chữ cái

          // Ci = (pi + ki mod m) mod 26
          int cipherChar = (plainChar - offset + (keyChar - 'A')) % AlphabetSize;
          cipherText.Append((char)(cipherChar + offset));
        }
        else
        {
          cipherText.Append(plainChar); // Giữ nguyên các ký tự không phải là chữ
        }
      }

      return cipherText.ToString();
    }

    public override string Decrypt(string cipherText, string key)
    {
      StringBuilder plainText = new();
      key = key.ToUpper();  // Đảm bảo key là in hoa
      int keyIndex = 0;     // Sử dụng chỉ số riêng cho key

      for (int i = 0; i < cipherText.Length; i++)
      {
        char cipherChar = cipherText[i];

        if (char.IsLetter(cipherChar))
        {
          char offset = char.IsUpper(cipherChar) ? 'A' : 'a';
          char keyChar = key[keyIndex % key.Length]; // Chỉ số key tuần hoàn
          keyIndex++; // Chỉ tăng khi gặp ký tự là chữ cái

          // pi = (Ci - ki + m) mod 26
          int plainChar = (cipherChar - offset - (keyChar - 'A') + AlphabetSize) % AlphabetSize;
          plainText.Append((char)(plainChar + offset));
        }
        else
        {
          plainText.Append(cipherChar); // Giữ nguyên các ký tự không phải là chữ
        }
      }

      return plainText.ToString();
    }

    public override IEnumerable<string> CrackingDecrypt(string cipherText)
    {
      throw new NotImplementedException();
    }

    // Hàm này không còn cần thiết vì xử lý key đã được thực hiện trong Encrypt và Decrypt
  }
}
