using Api.Interface;

namespace Api.Services
{
    public class Crypto : ICrypto
    {
        public string Encrypt(string password)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            return passwordHash;
        }
        public bool Decrypt(string password, string hash)
        {
            bool verifyPassword = BCrypt.Net.BCrypt.Verify(password, hash);
            return verifyPassword;
        }
    }
}
