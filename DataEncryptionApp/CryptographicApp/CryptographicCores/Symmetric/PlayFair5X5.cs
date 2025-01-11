using System.Text;

namespace CryptographicApp.CryptographicCores.Symmetric;

public class PlayFair5x5
{
  private bool _isGenerateKey = false;
  private Tuple<char, Tuple<int, int>>[,]? _matrixKey;

  public Dictionary<char, bool> _alphabetMap5x5 = new(new Dictionary<char, bool>
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

  private void PopulateMatrix5x5ToTableLayout(TableLayoutPanel table, char[,] matrix)
  {
    table.Controls.Clear();
    for (int i = 0; i < matrix.GetLength(0); i++)
    {
      for (int j = 0; j < matrix.GetLength(1); j++)
      {

        Label lbl = new()
        {
          Text = matrix[i, j].ToString(),
          TextAlign = ContentAlignment.MiddleCenter,
          Dock = DockStyle.Fill,
          BorderStyle = BorderStyle.FixedSingle
        };

        table.Controls.Add(lbl, j, i);
      }
    }
    _isGenerateKey = true;
  }

  private Tuple<int, int> CreateMatrix5x5Alphabet(char[,] matrixAlphabet, string key)
  {
    int row = 0;
    int column = 0;
    int r = 0, c = 0;
    for (int i = 0; i < key.Length; i++)
    {

      if (CheckKeyIsExist(key[i]) == false)
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

        _alphabetMap5x5[key[i]] = true;
        row = r;
        column = c;

      }
    }
    return new Tuple<int, int>(row, column);
  }

  private bool CheckKeyIsExist(char tmp)
  {
    try
    {
      return _alphabetMap5x5[tmp] != false;
    }
    catch
    {
      return false;
    }

  }

  private void FillRestOfAlpha(char[,] matrix, int r, int c)
  {
    if (r != 4 || c != 4)
    {
      foreach (char key in _alphabetMap5x5.Keys)
      {
        if (CheckKeyIsExist(key) == false)
        {
          matrix[r, c] = key;

          _alphabetMap5x5[key] = true;
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

  public void CreateMatrix5x5(TableLayoutPanel tableLayoutPanel1, string key)
  {
    ResetAlphabetMap();
    CheckKey(ref key);
    char[,] matrixAlphabet = new char[5, 5];
    Tuple<int, int> tuple1 = CreateMatrix5x5Alphabet(matrixAlphabet, key);

    int row_after_fill_key = tuple1.Item1;
    int column_after_fill_key = tuple1.Item2;

    FillRestOfAlpha(matrixAlphabet, row_after_fill_key, column_after_fill_key);

    _matrixKey = new Tuple<char, Tuple<int, int>>[5, 5];

    for (int i = 0; i < 5; i++)
    {
      for (int j = 0; j < 5; j++)
      {
        char character = matrixAlphabet[i, j];

        Tuple<char, Tuple<int, int>> tmp = new(character, new Tuple<int, int>(i, j));
        _matrixKey[i, j] = tmp;

      }
    }

    PopulateMatrix5x5ToTableLayout(tableLayoutPanel1, matrixAlphabet);
  }

  private void ResetAlphabetMap()
  {
    foreach (char x in _alphabetMap5x5.Keys)
    {
      _alphabetMap5x5[x] = false;
    }
  }

  private static void CheckKey(ref string key)
  {
    key = key.Trim();
    key = key.ToUpper();
    key = key.Replace('J', 'I');
    var sb = new StringBuilder();
    foreach (char c in key)
    {
      if (c >= 'A' && c <= 'Z')
      {
        sb.Append(c);
      }
    }
    key = sb.ToString();

  }

  private static void CheckPlainText(ref string plaintext)
  {
    plaintext = plaintext.Trim();
    plaintext = plaintext.ToUpper();
    plaintext = plaintext.Replace('J', 'I');

    var sb = new StringBuilder();
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

  private static void CheckCipherText(ref string cipherText)
  {
    cipherText = cipherText.Trim();
    cipherText = cipherText.ToUpper();
    cipherText = cipherText.Replace('J', 'I');
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

  private string EncryptUsingPlayFair(string plainText, Tuple<char, Tuple<int, int>>[,]? matrixKey)
  {
    if (matrixKey == null)
    {
      MessageBox.Show("Please create matrix key", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      return "";
    }
    string cipherText = "";
    char[] splitPlainText = plainText.ToCharArray();

    for (int i = 0; i < splitPlainText.Length; i += 2)
    {
      var value1 = ReturnValueOfCharacter(splitPlainText[i], matrixKey);

      int row1 = value1!.Item1;
      int column1 = value1.Item2;

      var value2 = ReturnValueOfCharacter(splitPlainText[i + 1], matrixKey);

      int row2 = value2!.Item1;
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

      char? charResult1 = ReturnAlphaByColumnRow(row1, column1, matrixKey);

      char? charResult2 = ReturnAlphaByColumnRow(row2, column2, matrixKey);

      cipherText += charResult1;
      cipherText += charResult2;
      cipherText += " ";

    }

    return cipherText;
  }

  private string DecryptUsingPlayFair(string cipherText, Tuple<char, Tuple<int, int>>[,]? matrixKey)
  {
    if (matrixKey == null)
    {
      MessageBox.Show("Please create matrix key", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      return "";
    }
    string plainText = "";
    char[] splitCipherText = cipherText.ToCharArray();

    for (int i = 0; i < splitCipherText.Length; i += 2)
    {
      var value1 = ReturnValueOfCharacter(splitCipherText[i], matrixKey);

      int row1 = value1!.Item1;
      int column1 = value1.Item2;

      var value2 = ReturnValueOfCharacter(splitCipherText[i + 1], matrixKey);

      int row2 = value2!.Item1;
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

      char? charResult1 = ReturnAlphaByColumnRow(row1, column1, matrixKey);

      char? charResult2 = ReturnAlphaByColumnRow(row2, column2, matrixKey);

      plainText += charResult1;
      plainText += charResult2;
      plainText += " ";

    }
    return plainText;

  }

  private char? ReturnAlphaByColumnRow(int row, int column, Tuple<char, Tuple<int, int>>[,] matrix)
  {
    char? resultChar = null;
    Tuple<int, int> target = new(row, column);
    for (int i = 0; i < matrix.GetLength(0); i++)
    {
      for (int j = 0; j < matrix.GetLength(1); j++)
      {
        if (matrix[i, j].Item2.Item1 == target.Item1 && matrix[i, j].Item2.Item2 == target.Item2)
        {
          resultChar = matrix[i, j].Item1;
          break;
        }
      }
    }
    return resultChar;
  }

  private static Tuple<int, int>? ReturnValueOfCharacter(char x, Tuple<char, Tuple<int, int>>[,] matrix)
  {
    Tuple<int, int>? result = null;

    for (int i = 0; i < matrix.GetLength(0); i++)
    {
      for (int j = 0; j < matrix.GetLength(1); j++)
      {
        if (matrix[i, j].Item1 == x)
        {
          result = matrix[i, j].Item2;
          break;
        }
      }
    }
    return result ?? null;
  }

  public string Encrypt(string plainText)
  {
    if (!_isGenerateKey)
    {
      MessageBox.Show("Please enter key", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      return "";
    }

    CheckPlainText(ref plainText);
    string cipherText = EncryptUsingPlayFair(plainText, _matrixKey);
    return cipherText;

  }

  public string Decrypt(string cipherText)
  {
    if (!_isGenerateKey)
    {
      MessageBox.Show("Please enter key", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      return "";
    }

    CheckCipherText(ref cipherText);
    string plainText = DecryptUsingPlayFair(cipherText, _matrixKey);
    return plainText;
  }

  public void ResetMatrixKey() => _matrixKey = null;
}