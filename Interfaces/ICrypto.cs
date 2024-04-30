namespace Api.Interface
{
    public interface ICrypto
    {
        string Encrypt(string password);
        bool Decrypt(string password, string hash);
    }
}