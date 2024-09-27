namespace DataEncryptionApp.DataEncryption.ShiftCipher;

public class CaesarWithWhiteSpaceWrapper(CaesarEncryption caesarEncryption) : AlphabetShiftEncryption
{
  private readonly CaesarEncryption _caesarEncryption = caesarEncryption;

  public override string Encrypt(string plainText, string shiftKey)
  {
    var blockEncryptedMessage = plainText
      .Split(' ')
      .Select(word => _caesarEncryption.Encrypt(word, shiftKey));

    return string.Join(" ", blockEncryptedMessage);
  }

  public override IEnumerable<string> CrackingDecrypt(string cipherText)
    => _caesarEncryption.CrackingDecrypt(cipherText);


  public override string Decrypt(string cipherText, string key)
      => _caesarEncryption.Decrypt(cipherText, key);
}