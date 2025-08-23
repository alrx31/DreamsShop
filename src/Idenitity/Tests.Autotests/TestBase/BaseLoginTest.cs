using Tests.Autotests.Core;
using Tests.Autotests.Helpers;
using Tests.Autotests.Models;
using FluentAssertions;

namespace Tests.Autotests.TestBase;

public abstract class BaseLoginTest : BaseWebDriverTest
{
    protected AuthenticationHelper AuthHelper { get; }
    protected ApiHelper ApiHelper { get; }

    protected BaseLoginTest() : base()
    {
        AuthHelper = new AuthenticationHelper(Driver);
        ApiHelper = new ApiHelper();
    }

    protected async Task<TestUser> CreateTestUser(UserRole role = UserRole.Consumer)
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        
        return role switch
        {
            UserRole.Consumer => TestUserFactory.CreateConsumerUser(
                email: $"testconsumer{timestamp}@test.com",
                password: "TestPassword123!"),
            UserRole.Producer => TestUserFactory.CreateProducerUser(
                email: $"testproducer{timestamp}@test.com", 
                password: "TestPassword123!"),
            UserRole.Admin => TestUserFactory.CreateAdminUser(
                email: $"testadmin{timestamp}@test.com",
                password: "TestPassword123!"),
            _ => throw new ArgumentException($"Unsupported role: {role}")
        };
    }

    protected async Task EnsureTestUserExists(TestUser user)
    {
        var registered = await ApiHelper.RegisterUserViaApiAsync(user);
        // It's okay if registration fails (user might already exist)
        Console.WriteLine(registered ? $"User {user.Email} registered" : $"User {user.Email} registration skipped (might already exist)");
    }

    protected async Task CleanupTestUser(TestUser user)
    {
        try
        {
            var token = await ApiHelper.LoginUserViaApiAsync(user);
            if (!string.IsNullOrEmpty(token))
            {
                await ApiHelper.DeleteUserViaApiAsync(user, token);
                Console.WriteLine($"Test user {user.Email} cleaned up");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to cleanup test user {user.Email}: {ex.Message}");
        }
    }

    protected void AssertLoginSuccess(LoginResult result, TestUser user)
    {
        result.Success.Should().BeTrue($"Login should succeed for user {user.Email}");
        result.ErrorMessage.Should().BeNullOrEmpty("No error message should be present on successful login");
    }

    protected void AssertLoginFailure(LoginResult result, string expectedErrorPattern = "")
    {
        result.Success.Should().BeFalse("Login should fail");
        result.ErrorMessage.Should().NotBeNullOrEmpty("Error message should be present on login failure");
        
        if (!string.IsNullOrEmpty(expectedErrorPattern))
        {
            result.ErrorMessage.Should().ContainEquivalentOf(expectedErrorPattern);
        }
    }

    public override void Dispose()
    {
        try
        {
            AuthHelper?.Dispose();
            ApiHelper?.Dispose();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during cleanup: {ex.Message}");
        }
        
        base.Dispose();
    }
}
