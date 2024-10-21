namespace AdvancedNumbersCalculator.Utilities.Encoders;

public interface IEncoderDecoder
{
  string Encode(byte[] data);
  byte[] Decode(string data);
}
