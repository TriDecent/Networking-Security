using System.Text;

namespace AdvancedNumbersCalculator.Utilities.Encoders;

public class UTF8EncoderDecoder : IEncoderDecoder
{
  public string Encode(byte[] data) => Encoding.UTF8.GetString(data);
  public byte[] Decode(string data) => Encoding.UTF8.GetBytes(data);
}