namespace HashCalculator;

internal interface IHashGenerator
{
  string GenerateHash(string data, DataFormat dataFormat);
}