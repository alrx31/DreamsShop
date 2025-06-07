namespace Domain.IServices;

public interface IPasswordManager
{
    byte CheckPassword(string password);
    bool Verify(string hash, string password);
    string HashPassword(string password);
}