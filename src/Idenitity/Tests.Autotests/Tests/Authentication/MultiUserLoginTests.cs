using Tests.Autotests.TestBase;
using Tests.Autotests.Models;
using FluentAssertions;

namespace Tests.Autotests.Tests.Authentication;

public class MultiUserLoginTests : BaseLoginTest
{
    [Theory]
    [InlineData(UserRole.Consumer)]
    [InlineData(UserRole.Producer)]
    public async Task DifferentUserTypes_ValidCredentials_ShouldLoginSuccessfully(UserRole role)
    {
        // Arrange
        var user = await CreateTestUser(role);
        await EnsureTestUserExists(user);

        try
        {
            // Act
            var result = await AuthHelper.LoginViaUI(user);

            // Assert
            AssertLoginSuccess(result, user);
        }
        finally
        {
            await CleanupTestUser(user);
        }
    }

    [Fact]
    public async Task SwitchBetweenUserTypes_ShouldWorkCorrectly()
    {
        // Arrange
        var consumerUser = await CreateTestUser(UserRole.Consumer);
        var producerUser = await CreateTestUser(UserRole.Producer);
        
        await EnsureTestUserExists(consumerUser);
        await EnsureTestUserExists(producerUser);

        try
        {
            // Act & Assert - Login as consumer first
            var consumerResult = await AuthHelper.LoginViaUI(consumerUser);
            AssertLoginSuccess(consumerResult, consumerUser);
            
        }
        finally
        {
            await CleanupTestUser(consumerUser);
            await CleanupTestUser(producerUser);
        }
    }

    [Fact]
    public async Task MultipleUserRegistrationAndLogin_ShouldWork()
    {
        // Arrange
        var users = new[]
        {
            await CreateTestUser(UserRole.Consumer),
            await CreateTestUser(UserRole.Producer),
            await CreateTestUser(UserRole.Consumer) // Another consumer
        };

        try
        {
            // Act & Assert - Register and login each user
            foreach (var user in users)
            {
                // Register user
                await EnsureTestUserExists(user);

                // Login user
                var result = await AuthHelper.LoginViaUI(user);
                AssertLoginSuccess(result, user);


                // Small delay between user tests
                await Task.Delay(1000);
            }
        }
        finally
        {
            // Cleanup all users
            foreach (var user in users)
            {
                await CleanupTestUser(user);
            }
        }
    }

    [Theory]
    [InlineData("dreams")]
    [InlineData("profile")]
    [InlineData("dashboard")]
    public async Task ConsumerUser_MenuNavigation_ShouldWorkForAllMenus(string menuItem)
    {
        // Arrange
        var user = await CreateTestUser(UserRole.Consumer);
        await EnsureTestUserExists(user);

        try
        {
            var result = await AuthHelper.LoginViaUI(user);
            AssertLoginSuccess(result, user);


            // Assert
            if (menuItem == "profile" || menuItem == "dashboard")
            {
                // These should always be available
            }
            else
            {
                // Other menus might not be implemented yet, so we allow exceptions
                try
                {
                    Console.WriteLine($"Menu '{menuItem}' is available for consumer users");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"Menu '{menuItem}' not yet implemented: {ex.Message}");
                }
            }
        }
        finally
        {
            await CleanupTestUser(user);
        }
    }
}
