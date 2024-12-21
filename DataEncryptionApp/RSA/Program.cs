using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

using var rsa = RSA.Create(1024);

var privateKey = rsa.ExportParameters(includePrivateParameters: true);
var publicKey = rsa.ExportParameters(includePrivateParameters: false);


Console.WriteLine("Public Key: " + GetKeyString(publicKey));
Console.WriteLine("Private Key: " + GetKeyString(privateKey));


string textToEncrypt = "Hello, World!";
string encryptedText = Encrypt(textToEncrypt, GetKeyString(publicKey));
string decryptedText = Decrypt(encryptedText, GetKeyString(privateKey));

Console.WriteLine("Encrypted Text: " + encryptedText);
Console.WriteLine("Decrypted Text: " + decryptedText);


string GetKeyString(RSAParameters key)
{
  var stringWriter = new StringWriter();
  var xmlSerializer = new XmlSerializer(typeof(RSAParameters));
  xmlSerializer.Serialize(stringWriter, key);
  return stringWriter.ToString();
}

string Encrypt(string text, string publicKey)
{
  using RSA rsa = RSA.Create(2048);
  rsa.FromXmlString(publicKey);
  var bytesToEncrypt = Encoding.UTF8.GetBytes(text);
  var encryptedData = rsa.Encrypt(bytesToEncrypt, RSAEncryptionPadding.Pkcs1);
  return Convert.ToBase64String(encryptedData);
}

string Decrypt(string text, string privateKey)
{
  using RSA rsa = RSA.Create(2048);
  rsa.FromXmlString(privateKey);
  var bytesToDecrypt = Convert.FromBase64String(text);
  var decryptedData = rsa.Decrypt(bytesToDecrypt, RSAEncryptionPadding.Pkcs1);
  return Encoding.UTF8.GetString(decryptedData);
}


// using System.Globalization;
// using System.Numerics;
// using System.Security.Cryptography;
// using System.Text;
// using System.Text.Unicode;

// var firstMessage = "d131dd02c5e6eec4693d9a0698aff95c2fcab58712467eab4004583eb8fb7f89" +
//   "55ad340609f4b30283e488832571415a085125e8f7cdc99fd91dbdf280373c5bd8" + "823e3156348f5bae6dacd436c919c6dd53e2b487da03fd02396306d248cda0e99f" +
//   "33420f577ee8ce54b67080a80d1ec69821bcb6a8839396f9652b6ff72a70";

// var secondMessage = "d131dd02c5e6eec4693d9a0698aff95c2fcab50712467eab4004583eb8fb7f8" +
//   "955ad340609f4b30283e4888325f1415a085125e8f7cdc99fd91dbd728037" +
//  "3c5bd8823e3156348f5bae6dacd436c919c6dd53e23487da03fd02396306d" +
//  "248cda0e99f33420f577ee8ce54b67080280d1ec69821bcb6a8839396f965a" +
//  "b6ff72a70";

// var firstMessageBytes = firstMessage.HexToBytes();
// var secondMessageBytes = secondMessage.HexToBytes();

// Console.WriteLine($"First message byte count: {firstMessageBytes.Length}");

// Console.WriteLine($"Second message byte count: {secondMessageBytes.Length}");

// var differentBytes = firstMessageBytes
//   .Zip(secondMessageBytes, (originalByte, comparisonByte) => 
//     (FirstMessageByte: originalByte, SecondMessageByte: comparisonByte))
//   .Where(bytePair => bytePair.FirstMessageByte != bytePair.SecondMessageByte);

// Console.WriteLine($"Number of different bytes: {differentBytes.Count()}");
// Console.WriteLine("Different pair bytes:");

// foreach (var (FirstMessageByte, SecondMessageByte) in differentBytes)
// {
//   Console.Write($"({FirstMessageByte}, {SecondMessageByte}) ");
// }
// Console.WriteLine();

// var md5 = new MD5HashGenerator();

// var firstMessageHash = md5.GenerateHash(firstMessage, DataFormat.Hex);
// var secondMessageHash = md5.GenerateHash(secondMessage, DataFormat.Hex);

// Console.WriteLine($"First message's hash: {firstMessageHash}");
// Console.WriteLine($"Second message's hash: {secondMessageHash}");



// // BigInteger p1 = 11, q1 = 17, e1 = 7;

// // BigInteger p2 = BigInteger.Parse("20079993872842322116151219");
// // BigInteger q2 = BigInteger.Parse("676717145751736242170789");
// // BigInteger e2 = 17;

// // BigInteger p3 = BigInteger.Parse("F7E75FDC469067FFDC4E847C51F452DF", NumberStyles.HexNumber);
// // BigInteger q3 = BigInteger.Parse("E85CED54AF57E53E092113E62F436F4F", NumberStyles.HexNumber);
// // BigInteger e3 = BigInteger.Parse("0D88C3", NumberStyles.HexNumber); ;

// // var rsa = new RSAEncryption();

// // rsa.GenerateKeyPair(p1, q1, e1);
// // rsa.GenerateKeyPair(p2, q2, e2);
// // rsa.GenerateKeyPair(p3, q3, e3);


// // Console.WriteLine(rsa.Encrypt(
// //   "The University of Information Technology", "(101776877529005912638346811918779931246783058062684819617574643018368103302097, 886979)"));

// // Console.WriteLine(rsa.Decrypt(
// //   "+J3bmtkyKySccvO5u4kNdO8gYwewn59vCyXi++3vpxA50aJZ498O1tWy1njqcJJ7O0OuqfNbnoWKBBkiBAVtWA==", "(101776877529005912638346811918779931246783058062684819617574643018368103302097, 24212225287904763939160097464943268930139828978795606022583874367720623008491)"
// // ));

// // Console.WriteLine(rsa.Decrypt(
// //   "wI21H9Pm2LksKn6aB7IQP/Pj9ru1HBCqVDvP3qlfQgIuG8/hS4uIIridpaI5Gjn5knelvu7NPkWTipxmI0Vyd1skCE4H/xdaw8a9tKWF1C7+9Spq7OPZgQ6k88aI7KCdZM6uCSQPnE95f0Kzn42g6aFb8J8CA5vFFtPQaCMA9fOGKBeHE9xaQI2ZdhK2RNc/zUmD/lAIzhP7N4wwj2NC71vDZPl+cmiyBdXTTDxTUZynn1EtKO/9eBG0Pb6VbAtFAyfJ3QrAl6PhPa0BXGVoe5JSNIR1KhSMoqJLoCWQvKLOSHl6p9ZEKX5kMyq7gW5A1BHo3+DCSgvzcbhOoxOYKVWXGwDYxDFB4OonPDyeKu0NgF3a0AGQP2KMMPaPQVpzcvxC+erO4S8f/R8wpf6tavs6yhsxur3hKBWZgarXB913djn5S2zhbyo8lZHjXsPVghrsxq6zmWOa32Q3HuIXCli/zBeKOjZ5+OBFr+JEo0ltoMdJN5u5LGRxEBEp6W7VWo29Y1jZHQVPE8hm/VXYLD3VesImKZPGVwKEn8wmpPkbFeQB+C89bLZ9P1UXKqMgqBFHFIuyXOfbHivqNiRARBfzDEcpu9YPc7VXiEVvlrgxWwAqjS0ruWesmOrsseTSZiNS7nMrt/M1S4HeEAHI5ViJqA9bl95zQO+wiOVw3iM=", "(210559680673619594419961015117731724317386967727044161558075741135964048067843543594049826963532439246705664420592475503618759639226382914432317246764080124166060228750081017139399229199991062314105240464455490690500582398782802688019481573213037354858463103655356297859874780487678941564166112282171747242154748777020992319178384415911503217134439682579201753199860404510228543264106984699145237936862216581442392937730790146331083685578381870525863493353400653412211268790608929386776055165101515818604195738896618506064488327322389431807607104400304118299381032724272954627368580842483766285105787008574444347276315515270331111643623992890359642883852964074898779821328897893902420095733167132700105443512158056107670395359064535937313232950318082310959541653919823912097945643690859331055880254955261600781022489712422418459887874707608148049824015141924288901398102571451394335891967045699658829402366662012360356074434957778788933455454910001819177930185701048948121460434297647917733143203908171284604587072986688292588112944803841301075897714868020766832040637082524844463253655392466208702100204261973153601690033329929584521676672037847238715412881417397021454693306559061995097351098026063843337104605664568370813203811917, 184535703173027276877173212464976380512042895850256122608773614044377481863263951865533056308717407159513454191152631574551027624063441345442254080937309765349741388203890983435855677366371464152086038368508323829138989444724168606920899081749808910249256836361943630013238747072200670867159146606513857002115481415166314232214609544176709496659151502941874225224535484594259682918969633644579474668958834456693268278444899730454833966287219730799304537402372298549478594466121505061083911081037212018755347218441568535832064575070816207558074511427747190789348740048889416588067338722397120468096206546095951294299526350236097124430618762613579428730324526810082997803291430144141815092760726300604029703041341033063700197375240712895742672436929811353948206873412132932006702721630332192832423681253778622257266941743144438178227266219578436069638783351351233549131770246777408207615785877125030368708686364145859526506679273735688793565631130604793011938673307714603591622071707885357622185425661375503321878646506930465455403747612085777466293293160838289320865074234503406037685141938007829451732361452598289030449301836874125805157162666616793078284253381295786256687319841426331616677312586623106440739857156156740768390881409)"
// // ));


// public class MD5HashGenerator
// {
//   public string GenerateHash(string data, DataFormat dataFormat)
//   => dataFormat switch
//   {
//     DataFormat.Text => GenerateHashFromText(data),
//     DataFormat.Hex => GenerateHashFromHex(data),
//     DataFormat.File => GenerateHashFromFile(data),
//     _ => throw new NotSupportedException($"Data format {dataFormat} is not supported."),
//   };

//   private static string GenerateHashFromText(string data)
//   {
//     var inputBytes = Encoding.UTF8.GetBytes(data);
//     var hashBytes = MD5.HashData(inputBytes);
//     return BitConverter.ToString(hashBytes).Replace("-", "");
//   }

//   private static string GenerateHashFromHex(string hexString)
//   {
//     var inputBytes = hexString.HexToBytes();
//     var hashBytes = MD5.HashData(inputBytes);
//     return BitConverter.ToString(hashBytes).Replace("-", "");
//   }

//   private static string GenerateHashFromFile(string filePath)
//   {
//     var inputBytes = File.ReadAllBytes(filePath);
//     var hashBytes = MD5.HashData(inputBytes);
//     return BitConverter.ToString(hashBytes).Replace("-", "");
//   }
// }

// public static class StringUtils
// {
//   public static byte[] HexToBytes(this string hex)
//   {
//     if (string.IsNullOrEmpty(hex))
//     {
//       throw new InvalidOperationException("Hex string cannot be null or empty");
//     }

//     if (hex.Length % 2 != 0)
//     {
//       throw new FormatException("Hex string must have an even length");
//     }

//     byte[] bytes = new byte[hex.Length / 2];
//     for (int i = 0; i < hex.Length; i += 2)
//     {
//       bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
//     }

//     return bytes;
//   }
// }

// public enum DataFormat
// {
//   Text, Hex, File
// }


// public class RSAEncryption
// {
//   private readonly AdvancedNumbersCalculator.LogicalMath.DeprecatedCalculator.AdvancedNumbersCalculator _calculator;
//   private readonly IEncoderDecoder _cipherTextEncoder;
//   private readonly IEncoderDecoder _plainTextEncoder;

//   public int E { get; private set; } // Public exponent e

//   public RSAEncryption()
//   {
//     _calculator = new AdvancedNumbersCalculator.LogicalMath.DeprecatedCalculator.AdvancedNumbersCalculator();
//     _cipherTextEncoder = new Base64EncoderDecoder();
//     _plainTextEncoder = new UTF8EncoderDecoder();
//   }

//   public RSAEncryption(
//     AdvancedNumbersCalculator.LogicalMath.DeprecatedCalculator.AdvancedNumbersCalculator calculator,
//     IEncoderDecoder cipherTextEncoder,
//     IEncoderDecoder plainTextEncoder)
//   {
//     _calculator = calculator;
//     _cipherTextEncoder = cipherTextEncoder;
//     _plainTextEncoder = plainTextEncoder;
//   }

//   public string Encrypt(string plainText, string publicKey)
//   {
//     // Parse public key components: N and E
//     var n = BigInteger.Parse(publicKey.Replace("(", "").Replace(")", "").Split(",")[0].Trim());
//     var e = BigInteger.Parse(publicKey.Replace("(", "").Replace(")", "").Split(",")[1].Trim());

//     // Convert the entire plaintext into a byte array using UTF-8 encoding
//     var plainBytes = _plainTextEncoder.Decode(plainText);

//     // Call the EncryptData method to handle the byte array encryption
//     byte[] encryptedData = EncryptData(plainBytes, n, e);

//     // encryptedData.ToList().ForEach(x => Console.Write(x.ToString("X2") + " "));

//     // Convert the encrypted byte array to Base64
//     return _cipherTextEncoder.Encode(encryptedData);
//   }

//   private byte[] EncryptData(byte[] data, BigInteger n, BigInteger e)
//   {
//     // Get the maximum size for each block in bytes
//     int blockSize = n.ToByteArray().Length - 1; // Max bytes for plaintext block
//     var blocks = new List<BigInteger>();

//     // Split the byte array into blocks
//     for (int i = 0; i < data.Length; i += blockSize)
//     {
//       // Take the current block of bytes
//       var block = data.Skip(i).Take(Math.Min(blockSize, data.Length - i)).ToArray();

//       // Ensure the block fits in BigInteger
//       if (block.Length > blockSize)
//       {
//         throw new ArgumentException("Block size exceeds the limit.");
//       }

//       // Convert the byte array to BigInteger for encryption
//       BigInteger plainNumber = new(block);

//       // Perform encryption: cipherNumber = (plainNumber^e) mod N
//       BigInteger cipherNumber = _calculator.ComputeModularExponentiation(plainNumber, e, n);

//       // Add the cipherNumber to the blocks list
//       blocks.Add(cipherNumber);
//     }

//     // Convert each cipherNumber to a byte array
//     return blocks.SelectMany(cipher => cipher.ToByteArray()).ToArray();
//   }

//   public string Decrypt(string cipherText, string privateKey)
//   {
//     // Parse private key components: N and D
//     var n = BigInteger.Parse(privateKey.Replace("(", "").Replace(")", "").Split(",")[0].Trim());
//     var d = BigInteger.Parse(privateKey.Replace("(", "").Replace(")", "").Split(",")[1].Trim());

//     // Convert the Base64 cipherText to a byte array
//     byte[] cipherBytes = _cipherTextEncoder.Decode(cipherText);
//     // cipherBytes.ToList().ForEach(x => Console.Write(x.ToString("X2") + " ")); 

//     // Console.WriteLine();

//     // Call the DecryptData method to handle the byte array decryption
//     byte[] decryptedData = DecryptData(cipherBytes, n, d);

//     decryptedData.ToList().ForEach(x => Console.Write(x.ToString("X2") + " "));

//     // Convert the decrypted byte array back to a string using UTF-8 encoding
//     return _plainTextEncoder.Encode(decryptedData);
//   }

//   private byte[] DecryptData(byte[] data, BigInteger n, BigInteger d)
//   {
//     var blocks = new List<BigInteger>();

//     // Split the byte array into blocks (BigInteger size)
//     int blockSize = n.ToByteArray().Length; // Max bytes for plaintext block // change block size here

//     for (int i = 0; i < data.Length; i += blockSize)
//     {
//       // Take the current block of bytes
//       var block = data.Skip(i).Take(Math.Min(blockSize, data.Length - i)).ToArray();

//       // Convert the byte array to BigInteger for decryption
//       BigInteger cipherNumber = new(block);

//       // Perform decryption: plainNumber = (cipherNumber^d) mod N
//       BigInteger plainNumber = _calculator.ComputeModularExponentiation(cipherNumber, d, n);

//       // Add the plainNumber to the blocks list
//       blocks.Add(plainNumber);
//     }

//     // Convert each plainNumber to a byte array and concatenate
//     return blocks.SelectMany(plain => plain.ToByteArray()).ToArray();
//   }

//   public void GenerateKeyPair(BigInteger? p = null, BigInteger? q = null, BigInteger? e = null)
//   {
//     // Use provided p, q, or generate them
//     p ??= _calculator.GenerateRandomPrimeNumber<BigInteger>();
//     q ??= _calculator.GenerateRandomPrimeNumber<BigInteger>();
//     var E = e ?? new BigInteger(65537);

//     var N = p.Value * q.Value; // Compute n = p * q
//     BigInteger phi = (p.Value - 1) * (q.Value - 1); // Compute φ(n) = (p-1)*(q-1)


//     // Ensure that e is coprime with φ(n)
//     if (_calculator.GetGCD(E, phi) != 1)
//     {
//       throw new ArgumentException("e must be coprime with φ(n). Please choose a different 'e'.");
//     }

//     // Compute the modular inverse of e mod φ(n), which gives us the private exponent d
//     var D = _calculator.ComputeModularInverse(E, phi);

//     // Key generation is done; N and D can now be used for encryption and decryption.

//     // Private key and public key are:
//     Console.WriteLine($"Public key: (N: {N}, E: {E})");
//     Console.WriteLine($"Private key: (N: {N}, D: {D})");
//   }
// }

// public interface IEncoderDecoder
// {
//   string Encode(byte[] data);
//   byte[] Decode(string data);
// }

// public class Base64EncoderDecoder : IEncoderDecoder
// {
//   public string Encode(byte[] data) => Convert.ToBase64String(data);
//   public byte[] Decode(string data) => Convert.FromBase64String(data);
// }

// public class UTF8EncoderDecoder : IEncoderDecoder
// {
//   public string Encode(byte[] data) => Encoding.UTF8.GetString(data);
//   public byte[] Decode(string data) => Encoding.UTF8.GetBytes(data);
// }