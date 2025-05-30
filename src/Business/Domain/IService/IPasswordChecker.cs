namespace Domain.IService;

public interface IPasswordChecker
{
    byte CheckPassword(string password);
}