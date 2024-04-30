using Api.Interface;
using BCrypt.Net;

namespace Api.Services
{
    public class Crypto : ICrypto
    {
        public string Encrypt(string password)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            return passwordHash;
        }
        public bool Decrypt(string password, string hash)
        {
            var verifyPassword = BCrypt.Net.BCrypt.Verify(password, hash);
            return verifyPassword;
        }
    }
}
