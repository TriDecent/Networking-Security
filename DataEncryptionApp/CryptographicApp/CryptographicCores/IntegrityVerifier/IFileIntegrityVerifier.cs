using CryptographicApp.Models;

namespace CryptographicApp.CryptographicCores.IntegrityVerifier;

public interface IFileIntegrityVerifier
{
  Task<bool> Verify(string filePath, RSAKey rsaKey);
}
