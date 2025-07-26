using System.Text;
using Domain.IServices;

namespace Infrastructure.Services;

public class PasswordManager: IPasswordManager
{
    public byte CheckPassword(string password)
    {
        return 255;
    }
    
    public bool Verify(string hash, string password)
    {
        return hash == HashPassword(password);
    }

    public string HashPassword(string password)
    {
        var data = Encoding.ASCII.GetBytes(password);
        data = System.Security.Cryptography.SHA256.HashData(data);

        return Encoding.ASCII.GetString(data);
    }
}