using CryptographicApp.Enums;

namespace CryptographicApp.CryptographicCores.HashGenerators;

internal interface IHashGenerator
{
  string GenerateHash(string data, DataFormat dataFormat);
}
