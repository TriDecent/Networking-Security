using CryptographicApp.Enums;

namespace CryptographicApp.CryptographicCores.HashGenerators;

public interface IHashGenerator
{
  string GenerateHash(string data, DataFormat dataFormat);
  string GenerateHash(Stream stream);
}
