using BLL.IService;

namespace BLL.Services;

public class PasswordHasher : IPasswordHasher
{
    public string Generate(string password)
    {
        var data = System.Text.Encoding.ASCII.GetBytes(password);
        data = System.Security.Cryptography.SHA256.HashData(data);
        return System.Text.Encoding.ASCII.GetString(data);
    }

    public bool Verify(string password, string hash)
    {
        return hash == Generate(password);
    }
}