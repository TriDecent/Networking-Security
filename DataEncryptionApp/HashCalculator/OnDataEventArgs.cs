namespace HashCalculator;

public class OnDataEventArgs : EventArgs
{
  public string Data { get; }
  public IEnumerable<HashAlgorithm> HashAlgorithms { get; }

  public OnDataEventArgs(string data, IEnumerable<HashAlgorithm> hashAlgorithms)
  {
    Data = data;
    HashAlgorithms = hashAlgorithms;
  }
}