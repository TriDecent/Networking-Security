namespace DataEncryptionApp.DataEncryption.AffineCipher;

public class AffineEncryption : ICrackingDataEncryption
{
  // Method to encrypt the plainText using Affine Cipher
  public string Encrypt(string plainText, string key)
  {
    var keys = ParseKey(key);
    int a = keys.key1;
    int b = keys.key2;

    string cipherText = "";
    foreach (char c in plainText.ToUpper())
    {
      if (char.IsLetter(c))
      {
        int x = c - 'A';
        int encryptedValue = (a * x + b) % 26;
        char encryptedChar = (char)(encryptedValue + 'A');
        cipherText += encryptedChar;
      }
      else
      {
        cipherText += c;
      }
    }

    return cipherText;
  }

  public string Decrypt(string cipherText, string key)
  {
    var keys = ParseKey(key);
    int a = keys.key1; 
    int b = keys.key2; 

    // Find modular inverse of a
    int aInverse = ModularInverse(a, 26);

    string plainText = "";
    foreach (char c in cipherText.ToUpper())
    {
      if (char.IsLetter(c))
      {
        int y = c - 'A';
        int decryptedValue = aInverse * ((y - b + 26) % 26) % 26;
        char decryptedChar = (char)(decryptedValue + 'A');
        plainText += decryptedChar;
      }
      else
      {
        plainText += c; // Preserve non-letter characters
      }
    }

    return plainText;
  }

  public IEnumerable<string> CrackingDecrypt(string cipherText)
  {
    throw new NotImplementedException();
  }

  private static (int key1, int key2) ParseKey(string key)
  {
    var keys = key.Split(',');
    if (keys.Length != 2)
    {
      throw new ArgumentException("Key must consist of two integers separated by a comma.");
    }

    int key1 = int.Parse(keys[0]);
    int key2 = int.Parse(keys[1]);

    if (!IsCoprime(key1, 26))
    {
      throw new ArgumentException("'a' must be coprime with 26.");
    }

    return (key1, key2);
  }

  private static int ModularInverse(int a, int m)
  {
    a %= m;
    for (int x = 1; x < m; x++)
    {
      if (a * x % m == 1)
      {
        return x;
      }
    }
    throw new ArgumentException("Modular inverse does not exist. Ensure 'a' is coprime with 26.");
  }

  private static bool IsCoprime(int a, int b)
  {
    return GCD(a, b) == 1;
  }

  private static int GCD(int a, int b)
  {
    while (b != 0)
    {
      int temp = b;
      b = a % b;
      a = temp;
    }
    return a;
  }
}