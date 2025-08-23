namespace Tests.Autotests.Models;

public enum UserRole
{
    Consumer,
    Producer,
    Admin
}

public class TestUser
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required UserRole Role { get; init; }
    public string? Name { get; set; }
}

public static class TestUserFactory
{
    public static TestUser CreateConsumerUser(string? email = null, string? password = null)
    {
        return new TestUser
        {
            Email = email ?? "consumer.test@example.com",
            Password = password ?? "TestPassword123!",
            Role = UserRole.Consumer,
            Name = "Test Consumer",
        };
    }
    
    public static TestUser CreateProducerUser(string? email = null, string? password = null)
    {
        return new TestUser
        {
            Email = email ?? "producer.test@example.com",
            Password = password ?? "TestPassword123!",
            Role = UserRole.Producer,
            Name = "Test Producer",
        };
    }
    
    public static TestUser CreateAdminUser(string? email = null, string? password = null)
    {
        return new TestUser
        {
            Email = email ?? "admin.test@example.com",
            Password = password ?? "AdminPassword123!",
            Role = UserRole.Admin,
            Name = "Test Admin",
        };
    }
}
