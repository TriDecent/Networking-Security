using System.Numerics;
using System.Text;

namespace DataEncryptionApp.DataEncryption.AsymmetricCipher;

public class RSAEncryption : ICrackingDataEncryption
{
  private readonly AdvancedNumbersCalculator.LogicalMath.AdvancedNumbersCalculator calculator;

  public int E { get; private set; } // Public exponent e

  public RSAEncryption()
  {
    calculator = new AdvancedNumbersCalculator.LogicalMath.AdvancedNumbersCalculator();
  }

  public string Encrypt(string plainText, string publicKey)
  {
    // The checking of the input must be done in the UI layer

    var n = BigInteger.Parse(publicKey.Replace("(", "").Replace(")", "").Split(",")[0].Trim());
    var e = BigInteger.Parse(publicKey.Replace("(", "").Replace(")", "").Split(",")[1].Trim());

    // Check if plaintext is numeric
    if (!BigInteger.TryParse(plainText, out BigInteger plainNumber))
    {
      plainNumber = new BigInteger(Encoding.UTF8.GetBytes(plainText));
    }

    // Ensure plainNumber is less than N
    if (plainNumber >= n)
    {
      throw new ArgumentException("The message is too large to be encrypted with the current key.");
    }

    // Encryption: cipherText = (plainNumber^e) mod N
    BigInteger cipherNumber = calculator.ComputeModularExponentiation(plainNumber, e, n);

    // Return as base64 string
    return Convert.ToBase64String(cipherNumber.ToByteArray());
  }


  public string Decrypt(string cipherText, string privateKey)
  {
    var n = BigInteger.Parse(privateKey.Replace("(", "").Replace(")", "").Split(",")[0].Trim());
    var d = BigInteger.Parse(privateKey.Replace("(", "").Replace(")", "").Split(",")[1].Trim());

    // Convert base64 cipherText to BigInteger
    BigInteger cipherNumber = new(Convert.FromBase64String(cipherText));

    // Decryption: plainNumber = (cipherNumber^d) mod N
    BigInteger plainNumber = calculator.ComputeModularExponentiation(cipherNumber, d, n);

    // Convert back to string
    return Encoding.UTF8.GetString(plainNumber.ToByteArray());
  }

  public void GenerateKeyPair(BigInteger? p = null, BigInteger? q = null, int e = 65537)
  {
    // Use provided p, q, or generate them
    p ??= calculator.GenerateRandomPrimeNumber<BigInteger>();
    q ??= calculator.GenerateRandomPrimeNumber<BigInteger>();

    var N = p.Value * q.Value; // Compute n = p * q
    BigInteger phi = (p.Value - 1) * (q.Value - 1); // Compute φ(n) = (p-1)*(q-1)
    var E = e;

    // Ensure that e is coprime with φ(n)
    if (calculator.GetGCD(E, phi) != 1)
    {
      throw new ArgumentException("e must be coprime with φ(n). Please choose a different 'e'.");
    }

    // Compute the modular inverse of e mod φ(n), which gives us the private exponent d
    var D = calculator.ComputeModularInverse(E, phi);

    // Key generation is done; N and D can now be used for encryption and decryption.

    // Private key and public key are:
    Console.WriteLine($"Public key: (N: {N}, E: {E})");
    Console.WriteLine($"Private key: (N: {N}, D: {D})");
  }


  public IEnumerable<string> CrackingDecrypt(string cipherText)
  {
    throw new NotImplementedException("Cracking RSA encryption is not implemented due to security reasons.");
  }
}