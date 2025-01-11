using System.Text;

namespace DataEncryptionApp.DataEncryption.PlayFair
{
  public class PlayFair_Algorithm : ICrackingDataEncryption
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
      //throw new NotImplementedException();
      checkKey(ref key);
      checkCipherText(ref cipherText);

      char[,] matrixAlphabet = new char[5, 5];
      Tuple<int, int> tuple1 = createMatrixAlphabet(matrixAlphabet, key);

      int row_after_fill_key = tuple1.Item1;
      int column_after_fill_key = tuple1.Item2;

      fillRestOfAlpha(matrixAlphabet, row_after_fill_key, column_after_fill_key);



      Tuple<char, Tuple<int, int>>[,] matrixKey = new Tuple<char, Tuple<int, int>>[5, 5];

      //create data for matrix key
      for (int i = 0; i < 5; i++)
      {
        for (int j = 0; j < 5; j++)
        {
          char character = matrixAlphabet[i, j];

          Tuple<char, Tuple<int, int>> tmp = new(character, new Tuple<int, int>(i, j));
          matrixKey[i, j] = tmp;

        }
      }

      //create data for matrix key


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
      //throw new NotImplementedException();

      checkPlainText(ref plainText);
      //Console.WriteLine(plainText);
      checkKey(ref key);
      char[,] matrixAlphabet = new char[5, 5];
      Tuple<int, int> tuple1 = createMatrixAlphabet(matrixAlphabet, key);

      int row_after_fill_key = tuple1.Item1;
      int column_after_fill_key = tuple1.Item2;

      fillRestOfAlpha(matrixAlphabet, row_after_fill_key, column_after_fill_key);



      Tuple<char, Tuple<int, int>>[,] matrixKey = new Tuple<char, Tuple<int, int>>[5, 5];

      //create data for matrix key
      for (int i = 0; i < 5; i++)
      {
        for (int j = 0; j < 5; j++)
        {
          char character = matrixAlphabet[i, j];

          Tuple<char, Tuple<int, int>> tmp = new(character, new Tuple<int, int>(i, j));
          matrixKey[i, j] = tmp;

        }
      }

      //create data for matrix key

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





    private bool checkKeyIsExist(char tmp)
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


    private Tuple<int, int> createMatrixAlphabet(char[,] matrixAlphabet, string key)
    {
      int row = 0;
      int column = 0;
      int r = 0, c = 0;
      for (int i = 0; i < key.Length; i++)
      {
        if (checkKeyIsExist(key[i]) == false)
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
      return new Tuple<int, int>(row, column);
    }

    private void fillRestOfAlpha(char[,] matrix, int r, int c)
    {
      if (r != 4 && c != 4)
      {
        foreach (char key in alphabetMap.Keys)
        {

          if (checkKeyIsExist(key) == false)
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


    private Tuple<int, int> returnValueOfCharacter(char x, Tuple<char, Tuple<int, int>>[,] matrixx)
    {
      Tuple<int, int> result = null;

      for (int i = 0; i < matrixx.GetLength(0); i++)
      {
        for (int j = 0; j < matrixx.GetLength(1); j++)
        {
          if (matrixx[i, j].Item1 == x)
          {
            result = matrixx[i, j].Item2;
            break;
          }
        }
      }
      return result != null ? result : null;
    }

    private char? returnAlphaByColumnRow(int row, int column, Tuple<char, Tuple<int, int>>[,] matrixx)
    {
      char? resultChar = null;
      Tuple<int, int> target = new(row, column);
      for (int i = 0; i < matrixx.GetLength(0); i++)
      {
        for (int j = 0; j < matrixx.GetLength(1); j++)
        {
          if (matrixx[i, j].Item2.Item1 == target.Item1 && matrixx[i, j].Item2.Item2 == target.Item2)
          {
            resultChar = matrixx[i, j].Item1;
            break;
          }
        }
      }
      return resultChar;
    }

    private string EncryptUsingPlayFair(string plainText, Tuple<char, Tuple<int, int>>[,] matrixKey)
    {
      string cipherText = "";
      char[] splitPlainText = plainText.ToCharArray();

      for (int i = 0; i < splitPlainText.Length; i += 2)
      {
        Tuple<int, int> value1 = returnValueOfCharacter(splitPlainText[i], matrixKey);

        int row1 = value1.Item1;
        int column1 = value1.Item2;

        Tuple<int, int> value2 = returnValueOfCharacter(splitPlainText[i + 1], matrixKey);

        int row2 = value2.Item1;
        int column2 = value2.Item2;

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

        char? charResult1 = returnAlphaByColumnRow(row1, column1, matrixKey);

        char? charResult2 = returnAlphaByColumnRow(row2, column2, matrixKey);

        cipherText += charResult1;
        cipherText += charResult2;
        cipherText += " ";

      }

      return cipherText;
    }
    private void checkPlainText(ref string plaintext)
    {
      //check format
      plaintext = plaintext.Trim();
      plaintext = plaintext.ToUpper();
      //plaintext = plaintext.Replace(" ", "");
      plaintext = plaintext.Replace('J', 'I');
      /*plaintext = plaintext.Replace("\n", "");
      plaintext = plaintext.Replace("\r", "");*/


      StringBuilder sb = new();
      foreach (char c in plaintext)
      {
        if (c >= 'A' && c <= 'Z')
        {
          sb.Append(c);
        }
      }
      plaintext = sb.ToString();
      /*for (int i=0;i< plaintext.Length; i++)
      {
          if (!(plaintext[i] >= 'A' && plaintext[i] <= 'Z'))
          {
              plaintext=plaintext.Remove(i,1);
          }
      }*/
      //TTT

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


    private void checkKey(ref string key)
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

    private void checkCipherText(ref string cipherText)
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

    private string DecryptUsingPlayFair(string cipherText, Tuple<char, Tuple<int, int>>[,] matrixKey)
    {
      string plainText = "";
      char[] splitCipherText = cipherText.ToCharArray();

      for (int i = 0; i < splitCipherText.Length; i += 2)
      {
        Tuple<int, int> value1 = returnValueOfCharacter(splitCipherText[i], matrixKey);

        int row1 = value1.Item1;
        int column1 = value1.Item2;

        Tuple<int, int> value2 = returnValueOfCharacter(splitCipherText[i + 1], matrixKey);

        int row2 = value2.Item1;
        int column2 = value2.Item2;

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

        char? charResult1 = returnAlphaByColumnRow(row1, column1, matrixKey);

        char? charResult2 = returnAlphaByColumnRow(row2, column2, matrixKey);

        plainText += charResult1;
        plainText += charResult2;
        plainText += " ";

      }
      return plainText;

    }
  }
}
