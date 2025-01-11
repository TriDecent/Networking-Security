using System.Text;

namespace DataEncryptionApp.DataEncryption.PlayFair;

// When using this class, the key must be in uppercase
// The key passed from user has not been standardized.
// But it is acceptable.
public class PlayFairEncryption : ICrackingDataEncryption
{
  public Dictionary<char, bool> alphabetMap = new(new Dictionary<char, bool>
  {
    ['A'] = false,
    ['B'] = false,
    ['C'] = false,
    ['D'] = false,
    ['E'] = false,
    ['F'] = false,
    ['G'] = false,
    ['H'] = false,
    ['I'] = false,
    ['K'] = false,
    ['L'] = false,
    ['M'] = false,
    ['N'] = false,
    ['O'] = false,
    ['P'] = false,
    ['Q'] = false,
    ['R'] = false,
    ['S'] = false,
    ['T'] = false,
    ['U'] = false,
    ['V'] = false,
    ['W'] = false,
    ['X'] = false,
    ['Y'] = false,
    ['Z'] = false
  });

  public IEnumerable<string> CrackingDecrypt(string cipherText)
  {
    throw new NotImplementedException();
  }

  public string Decrypt(string cipherText, string key)
  {
    NormalizeKey(ref key);
    NormalizeCipherText(ref cipherText);

    char[,] matrixAlphabet = new char[5, 5];
    (int row_after_fill_key, int column_after_fill_key) = createMatrixAlphabet(matrixAlphabet, key);

    fillRestOfAlpha(matrixAlphabet, row_after_fill_key, column_after_fill_key);

    (char character, (int, int) position)[,] matrixKey = new (char, (int, int))[5, 5];

    //create data for matrix key
    for (int i = 0; i < 5; i++)
    {
      for (int j = 0; j < 5; j++)
      {
        char character = matrixAlphabet[i, j];
        matrixKey[i, j] = (character, (i, j));
      }
    }

    Console.WriteLine("KEY MATRIX");
    for (int i = 0; i < 5; i++)
    {
      for (int j = 0; j < 5; j++)
      {
        Console.Write(matrixAlphabet[i, j] + "  ");
      }
      Console.WriteLine();
    }

    string plaintext = DecryptUsingPlayFair(cipherText, matrixKey);

    return plaintext;
  }

  public string Encrypt(string plainText, string key)
  {
    NormalizePlainText(ref plainText);
    NormalizeKey(ref key);
    char[,] matrixAlphabet = new char[5, 5];
    (int row_after_fill_key, int column_after_fill_key) = createMatrixAlphabet(matrixAlphabet, key);

    fillRestOfAlpha(matrixAlphabet, row_after_fill_key, column_after_fill_key);

    (char character, (int, int) position)[,] matrixKey = new (char, (int, int))[5, 5];

    //create data for matrix key
    for (int i = 0; i < 5; i++)
    {
      for (int j = 0; j < 5; j++)
      {
        char character = matrixAlphabet[i, j];
        matrixKey[i, j] = (character, (i, j));
      }
    }

    Console.WriteLine("KEY MATRIX");
    for (int i = 0; i < 5; i++)
    {
      for (int j = 0; j < 5; j++)
      {
        Console.Write(matrixAlphabet[i, j] + "  ");
      }
      Console.WriteLine();
    }

    string cipherText = EncryptUsingPlayFair(plainText, matrixKey);

    return cipherText;
  }

  private bool DoesKeyExist(char tmp)
  {
    try
    {
      return alphabetMap[tmp] != false;
    }
    catch
    {
      return false;
    }
  }

  private (int, int) createMatrixAlphabet(char[,] matrixAlphabet, string key)
  {
    int row = 0;
    int column = 0;
    int r = 0, c = 0;
    for (int i = 0; i < key.Length; i++)
    {
      if (DoesKeyExist(key[i]) == false)
      {
        matrixAlphabet[r, c] = key[i];
        if (c == 4)
        {
          r++;
          c = 0;
        }
        else
        {
          c++;
        }

        alphabetMap[key[i]] = true;
        row = r;
        column = c;
      }
    }
    return (row, column);
  }

  private void fillRestOfAlpha(char[,] matrix, int r, int c)
  {
    if (r != 4 && c != 4)
    {
      foreach (char key in alphabetMap.Keys)
      {
        if (DoesKeyExist(key) == false)
        {
          matrix[r, c] = key;
          alphabetMap[key] = true;
          if (c == 4)
          {
            r++;
            c = 0;
          }
          else
          {
            c++;
          }
        }
      }
    }
  }

  private (int, int)? ReturnValueOfCharacter(char x, (char character, (int, int) position)[,] matrix)
  {
    for (int i = 0; i < matrix.GetLength(0); i++)
    {
      for (int j = 0; j < matrix.GetLength(1); j++)
      {
        if (matrix[i, j].character == x)
        {
          return matrix[i, j].position;
        }
      }
    }
    return null;
  }

  private char? ReturnAlphaByColumnRow(int row, int column, (char character, (int, int) position)[,] matrix)
  {
    for (int i = 0; i < matrix.GetLength(0); i++)
    {
      for (int j = 0; j < matrix.GetLength(1); j++)
      {
        if (matrix[i, j].position == (row, column))
        {
          return matrix[i, j].character;
        }
      }
    }
    return null;
  }

  private string EncryptUsingPlayFair(string plainText, (char character, (int, int) position)[,] matrixKey)
  {
    string cipherText = "";
    char[] splitPlainText = plainText.ToCharArray();

    for (int i = 0; i < splitPlainText.Length; i += 2)
    {
      (int row1, int column1) = ReturnValueOfCharacter(splitPlainText[i], matrixKey).Value;
      (int row2, int column2) = ReturnValueOfCharacter(splitPlainText[i + 1], matrixKey).Value;

      if (row1 == row2)
      {
        column1++;
        column2++;
        if (column1 == 5)
        {
          column1 = 0;
        }
        if (column2 == 5)
        {
          column2 = 0;
        }
      }
      else if (column1 == column2)
      {
        row1++;
        row2++;
        if (row1 == 5)
        {
          row1 = 0;
        }
        if (row2 == 5)
        {
          row2 = 0;
        }
      }
      else
      {
        int distance = Math.Abs(column2 - column1);
        if (column1 > column2)
        {
          column1 -= distance;
          column2 += distance;
        }
        else
        {
          column1 += distance;
          column2 -= distance;
        }
      }

      char? charResult1 = ReturnAlphaByColumnRow(row1, column1, matrixKey);
      char? charResult2 = ReturnAlphaByColumnRow(row2, column2, matrixKey);

      cipherText += charResult1;
      cipherText += charResult2;
      cipherText += " ";
    }

    return cipherText;
  }

  private static void NormalizePlainText(ref string plaintext)
  {
    plaintext = plaintext.Trim();
    plaintext = plaintext.ToUpper();
    plaintext = plaintext.Replace('J', 'I');

    StringBuilder sb = new();
    foreach (char c in plaintext)
    {
      if (c >= 'A' && c <= 'Z')
      {
        sb.Append(c);
      }
    }
    plaintext = sb.ToString();

    for (int i = 0; i < plaintext.Length;)
    {
      if (plaintext[i].Equals(plaintext[i + 1]))
      {
        plaintext = plaintext.Insert(i + 1, "X");
        i += 2;
      }
      else
      {
        i += 2;
      }

      if (i == plaintext.Length - 1)
      {
        break;
      }
    }

    if (plaintext.Length % 2 != 0)
    {
      plaintext += "X";
    }
  }

  private static void NormalizeKey(ref string key)
  {
    key = key.Trim();
    key = key.ToUpper();
    key = key.Replace('J', 'I');
    StringBuilder sb = new();
    foreach (char c in key)
    {
      if (c >= 'A' && c <= 'Z')
      {
        sb.Append(c);
      }
    }
    key = sb.ToString();
  }

  private static void NormalizeCipherText(ref string cipherText)
  {
    cipherText = cipherText.Trim();
    cipherText = cipherText.ToUpper();
    StringBuilder sb = new();
    foreach (char c in cipherText)
    {
      if (c >= 'A' && c <= 'Z')
      {
        sb.Append(c);
      }
    }
    cipherText = sb.ToString();
    for (int i = 0; i < cipherText.Length;)
    {
      if (cipherText[i].Equals(cipherText[i + 1]))
      {
        cipherText = cipherText.Insert(i + 1, "X");
        i += 2;
      }
      else
      {
        i += 2;
      }

      if (i == cipherText.Length - 1)
      {
        break;
      }
    }

    if (cipherText.Length % 2 != 0)
    {
      cipherText += "X";
    }
  }

  private string DecryptUsingPlayFair(string cipherText, (char character, (int, int) position)[,] matrixKey)
  {
    string plainText = "";
    char[] splitCipherText = cipherText.ToCharArray();

    for (int i = 0; i < splitCipherText.Length; i += 2)
    {
      (int row1, int column1) = ReturnValueOfCharacter(splitCipherText[i], matrixKey).Value;
      (int row2, int column2) = ReturnValueOfCharacter(splitCipherText[i + 1], matrixKey).Value;

      if (row1 == row2)
      {
        column1--;
        column2--;
        if (column1 == -1)
        {
          column1 = 4;
        }
        if (column2 == -1)
        {
          column2 = 4;
        }
      }
      else if (column1 == column2)
      {
        row1--;
        row2--;
        if (row1 == -1)
        {
          row1 = 4;
        }
        if (row2 == -1)
        {
          row2 = 4;
        }
      }
      else
      {
        int distance = Math.Abs(column2 - column1);
        if (column1 > column2)
        {
          column1 -= distance;
          column2 += distance;
        }
        else
        {
          column1 += distance;
          column2 -= distance;
        }
      }

      char? charResult1 = ReturnAlphaByColumnRow(row1, column1, matrixKey);
      char? charResult2 = ReturnAlphaByColumnRow(row2, column2, matrixKey);

      plainText += charResult1;
      plainText += charResult2;
      plainText += " ";
    }
    return plainText;
  }
}
