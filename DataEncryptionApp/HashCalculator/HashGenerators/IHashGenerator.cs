using HashCalculator.Enums;

namespace HashCalculator.HashGenerators;

internal interface IHashGenerator
{
  string GenerateHash(string data, DataFormat dataFormat);
}