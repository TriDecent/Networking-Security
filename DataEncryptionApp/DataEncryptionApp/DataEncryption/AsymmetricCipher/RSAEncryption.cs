using AdvancedNumbersCalculator.LogicalMath.ModularArithmeticCalculators;
using AdvancedNumbersCalculator.LogicalMath.PrimeNumbersCalculators;
using AdvancedNumbersCalculator.Utilities.Encoders;
using System.Numerics;

namespace DataEncryptionApp.DataEncryption.AsymmetricCipher;

public class RSAEncryption : ICrackingDataEncryption
{
  private readonly IPrimeNumberCalculator _primeNumberCalculator;
  private readonly IModularArithmeticCalculator _modularArithmeticCalculator;
  private readonly IEncoderDecoder _cipherTextEncoder;
  private readonly IEncoderDecoder _plainTextEncoder;

  public int E { get; private set; } // Public exponent e

  public RSAEncryption()
  {
    _primeNumberCalculator = new PrimeNumberCalculator();
    _modularArithmeticCalculator = new ModularArithmeticCalculator();
    _cipherTextEncoder = new Base64EncoderDecoder();
    _plainTextEncoder = new UTF8EncoderDecoder();
  }

  public RSAEncryption(
    IPrimeNumberCalculator primeNumberCalculator,
    IModularArithmeticCalculator modularArithmeticCalculator,
    IEncoderDecoder cipherTextEncoder,
    IEncoderDecoder plainTextEncoder)
  {
    _primeNumberCalculator = primeNumberCalculator;
    _modularArithmeticCalculator = modularArithmeticCalculator;
    _cipherTextEncoder = cipherTextEncoder;
    _plainTextEncoder = plainTextEncoder;
  }

  public string Encrypt(string plainText, string publicKey)
  {
    // Parse public key components: N and E
    var n = BigInteger.Parse(publicKey.Replace("(", "").Replace(")", "").Split(",")[0].Trim());
    var e = BigInteger.Parse(publicKey.Replace("(", "").Replace(")", "").Split(",")[1].Trim());

    // Convert the entire plaintext into a byte array using UTF-8 encoding
    var plainBytes = _plainTextEncoder.Decode(plainText);

    // Call the EncryptData method to handle the byte array encryption
    byte[] encryptedData = EncryptData(plainBytes, n, e);

    // encryptedData.ToList().ForEach(x => Console.Write(x.ToString("X2") + " "));

    // Convert the encrypted byte array to Base64
    return _cipherTextEncoder.Encode(encryptedData);
  }

  private byte[] EncryptData(byte[] data, BigInteger n, BigInteger e)
  {
    // Get the maximum size for each block in bytes
    int blockSize = n.ToByteArray().Length - 1; // Max bytes for plaintext block
    var blocks = new List<BigInteger>();

    // Split the byte array into blocks
    for (int i = 0; i < data.Length; i += blockSize)
    {
      // Take the current block of bytes
      var block = data.Skip(i).Take(Math.Min(blockSize, data.Length - i)).ToArray();

      // Ensure the block fits in BigInteger
      if (block.Length > blockSize)
      {
        throw new ArgumentException("Block size exceeds the limit.");
      }

      // Convert the byte array to BigInteger for encryption
      BigInteger plainNumber = new(block);

      // Perform encryption: cipherNumber = (plainNumber^e) mod N
      BigInteger cipherNumber = _modularArithmeticCalculator.ComputeModularExponentiation(plainNumber, e, n);

      // Add the cipherNumber to the blocks list
      blocks.Add(cipherNumber);
    }

    // Convert each cipherNumber to a byte array
    return blocks.SelectMany(cipher => cipher.ToByteArray()).ToArray();
  }

  public string Decrypt(string cipherText, string privateKey)
  {
    // Parse private key components: N and D
    var n = BigInteger.Parse(privateKey.Replace("(", "").Replace(")", "").Split(",")[0].Trim());
    var d = BigInteger.Parse(privateKey.Replace("(", "").Replace(")", "").Split(",")[1].Trim());

    // Convert the Base64 cipherText to a byte array
    byte[] cipherBytes = _cipherTextEncoder.Decode(cipherText);
    // cipherBytes.ToList().ForEach(x => Console.Write(x.ToString("X2") + " ")); 

    // Console.WriteLine();

    // Call the DecryptData method to handle the byte array decryption
    byte[] decryptedData = DecryptData(cipherBytes, n, d);

    // decryptedData.ToList().ForEach(x => Console.Write(x.ToString("X2") + " "));

    // Convert the decrypted byte array back to a string using UTF-8 encoding
    return _plainTextEncoder.Encode(decryptedData);
  }

  private byte[] DecryptData(byte[] data, BigInteger n, BigInteger d)
  {
    var blocks = new List<BigInteger>();

    // Split the byte array into blocks (BigInteger size)
    int blockSize = 512; // Max bytes for plaintext block

    for (int i = 0; i < data.Length; i += blockSize)
    {
      // Take the current block of bytes
      var block = data.Skip(i).Take(Math.Min(blockSize, data.Length - i)).ToArray();

      // Convert the byte array to BigInteger for decryption
      BigInteger cipherNumber = new(block);

      // Perform decryption: plainNumber = (cipherNumber^d) mod N
      BigInteger plainNumber = _modularArithmeticCalculator.ComputeModularExponentiation(cipherNumber, d, n);

      // Add the plainNumber to the blocks list
      blocks.Add(plainNumber);
    }

    // Convert each plainNumber to a byte array and concatenate
    return blocks.SelectMany(plain => plain.ToByteArray()).ToArray();
  }

  public void GenerateKeyPair(BigInteger? p = null, BigInteger? q = null, BigInteger? e = null)
  {
    // Use provided p, q, or generate them
    p ??= _primeNumberCalculator.GenerateRandomPrimeNumber<BigInteger>();
    q ??= _primeNumberCalculator.GenerateRandomPrimeNumber<BigInteger>();
    var E = e ?? new BigInteger(65537);

    var N = p.Value * q.Value; // Compute n = p * q
    BigInteger phi = (p.Value - 1) * (q.Value - 1); // Compute φ(n) = (p-1)*(q-1)


    // Ensure that e is coprime with φ(n)
    if (_modularArithmeticCalculator.GetGCD(E, phi) != 1)
    {
      throw new ArgumentException("e must be coprime with φ(n). Please choose a different 'e'.");
    }

    // Compute the modular inverse of e mod φ(n), which gives us the private exponent d
    var D = _modularArithmeticCalculator.ComputeModularInverse(E, phi);

    // Key generation is done; N and D can now be used for encryption and decryption.

    // Private key and public key are:
    Console.WriteLine($"Public key: (N: {N}, E: {E})");
    Console.WriteLine($"Private key: (N: {N}, D: {D})");
  }

  public IEnumerable<string> CrackingDecrypt(string cipherText)
  {
    throw new NotImplementedException();
  }
}