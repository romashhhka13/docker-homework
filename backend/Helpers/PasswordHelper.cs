using System.Security.Cryptography;
using System.Text;

namespace ShpaginApp.Helpers
{
  public static class PasswordHelper
  {
    public static string HashPassword(string password)
    {
      var bytes = Encoding.UTF8.GetBytes(password);
      var hash = SHA256.HashData(bytes);
      return Convert.ToBase64String(hash);
    }

    public static bool VerifyPassword(string passwordHash, string storedHash)
    {
      return storedHash == passwordHash;
    }
  }
}
