using System.Collections.ObjectModel;

namespace DataEncryptionApp.DataEncryption.ShiftCipher;

public class CaesarEncryption : AlphabetShiftEncryption
{
  private readonly ReadOnlyDictionary<char, int> _alphabetMapping = new(new Dictionary<char, int>
  {
    ['A'] = 1,
    ['B'] = 2,
    ['C'] = 3,
    ['D'] = 4,
    ['E'] = 5,
    ['F'] = 6,
    ['G'] = 7,
    ['H'] = 8,
    ['I'] = 9,
    ['J'] = 10,
    ['K'] = 11,
    ['L'] = 12,
    ['M'] = 13,
    ['N'] = 14,
    ['O'] = 15,
    ['P'] = 16,
    ['Q'] = 17,
    ['R'] = 18,
    ['S'] = 19,
    ['T'] = 20,
    ['U'] = 21,
    ['V'] = 22,
    ['W'] = 23,
    ['X'] = 24,
    ['Y'] = 25,
    ['Z'] = 26
  });

  public override string Encrypt(string plainText, string key)
  {
    string cipherText = "";

    foreach (var letter in plainText)
    {
      var normalizedLetter = char.ToUpper(letter);

      if (!_alphabetMapping.ContainsKey(normalizedLetter))
      {
        cipherText += letter;
        continue;
      }

      int letterNum = _alphabetMapping[normalizedLetter];
      int shiftedLetterNum = (letterNum + ParseStringKeyToInt(key)) % 26;

      if (shiftedLetterNum == 0)
      {
        shiftedLetterNum = 26;
      }

      var encryptedChar = _alphabetMapping
        .FirstOrDefault(keyValuePair => keyValuePair.Value == shiftedLetterNum).Key;

      if (char.IsLower(letter))
      {
        cipherText += char.ToLower(encryptedChar);
        continue;
      }

      cipherText += encryptedChar;
    }

    return cipherText;
  }

  public override IEnumerable<string> CrackingDecrypt(string cipherText)
  {
    var results = new List<string>();

    if (cipherText.Length == 0)
    {
      return results;
    }

    for (int shift = 1; shift < 26; shift++)
    {
      var blockDecryptedMessage = cipherText
        .Split(' ')
        .Select(block => Decrypt(block, shift.ToString()));

      results.Add($"Key {shift}: {string.Join(" ", blockDecryptedMessage)}");
    }

    return results;
  }

  public override string Decrypt(string cipherText, string shift)
  {
    var firstChar = char.ToUpper(shift.FirstOrDefault());

    var asIntKey = char.IsLetter(firstChar) ?
      _alphabetMapping[firstChar] :
      ParseStringKeyToInt(shift);

    var result = "";

    foreach (var letter in cipherText)
    {
      var normalizedLetter = char.ToUpper(letter);
      if (!_alphabetMapping.ContainsKey(normalizedLetter))
      {
        result += letter;
        continue;
      }

      int letterNum = _alphabetMapping[normalizedLetter];
      int shiftedLetterNum = (letterNum - asIntKey + 26) % 26;

      if (shiftedLetterNum == 0)
      {
        shiftedLetterNum = 26;
      }

      var decryptedChar = _alphabetMapping
        .FirstOrDefault(keyValuePair => keyValuePair.Value == shiftedLetterNum).Key;

      if (char.IsLower(letter))
      {
        result += char.ToLower(decryptedChar);
        continue;
      }

      result += decryptedChar;
    }

    return result;
  }
}