namespace AdvancedNumbersCalculator.Utilities.Encoders;

public class Base64EncoderDecoder : IEncoderDecoder
{
  public string Encode(byte[] data) => Convert.ToBase64String(data);
  public byte[] Decode(string data) => Convert.FromBase64String(data);
}
