using System.Globalization;
using System.Numerics;
using System.Text;

BigInteger p1 = 11, q1 = 17, e1 = 7;

BigInteger p2 = BigInteger.Parse("20079993872842322116151219");
BigInteger q2 = BigInteger.Parse("676717145751736242170789");
BigInteger e2 = 17;

BigInteger p3 = BigInteger.Parse("F7E75FDC469067FFDC4E847C51F452DF", NumberStyles.HexNumber);
BigInteger q3 = BigInteger.Parse("E85CED54AF57E53E092113E62F436F4F", NumberStyles.HexNumber);
BigInteger e3 = BigInteger.Parse("0D88C3", NumberStyles.HexNumber); ;

var rsa = new RSAEncryption();

rsa.GenerateKeyPair(p1, q1, e1);
rsa.GenerateKeyPair(p2, q2, e2);
rsa.GenerateKeyPair(p3, q3, e3);


Console.WriteLine(rsa.Encrypt(
  "The University of Information Technology", "(20079993872842322116151219, 676717145751736242170789)"));


public class RSAEncryption
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

  public void GenerateKeyPair(BigInteger? p = null, BigInteger? q = null, BigInteger? e = null)
  {
    // Use provided p, q, or generate them
    p ??= calculator.GenerateRandomPrimeNumber<BigInteger>();
    q ??= calculator.GenerateRandomPrimeNumber<BigInteger>();
    var E = e ?? new BigInteger(65537);

    var N = p.Value * q.Value; // Compute n = p * q
    BigInteger phi = (p.Value - 1) * (q.Value - 1); // Compute φ(n) = (p-1)*(q-1)


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
}