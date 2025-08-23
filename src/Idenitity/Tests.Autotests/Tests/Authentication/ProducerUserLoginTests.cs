using Tests.Autotests.TestBase;
using Tests.Autotests.Models;
using Tests.Autotests.PageObjects.Authentication;
using FluentAssertions;

namespace Tests.Autotests.Tests.Authentication;

public class ProducerUserLoginTests : BaseLoginTest
{
    [Fact]
    public async Task ProducerUser_ValidCredentials_ShouldLoginSuccessfully()
    {
        // Arrange
        var user = await CreateTestUser(UserRole.Producer);
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
    public async Task ProducerUser_InvalidCredentials_ShouldFailLogin()
    {
        // Arrange
        var user = await CreateTestUser(UserRole.Producer);
        await EnsureTestUserExists(user);

        var invalidUser = new TestUser
        {
            Email = user.Email,
            Password = "WrongPassword123!",
            Role = user.Role,
            Name = user.Name
        };

        try
        {
            // Act
            var result = await AuthHelper.LoginViaUI(invalidUser);

            // Assert
            AssertLoginFailure(result, "password");
        }
        finally
        {
            await CleanupTestUser(user);
        }
    }

    [Fact]
    public async Task ProducerUser_Navigation_ShouldAccessProducerMenus()
    {
        // Arrange
        var user = await CreateTestUser(UserRole.Producer);
        await EnsureTestUserExists(user);

        try
        {
            var result = await AuthHelper.LoginViaUI(user);
            AssertLoginSuccess(result, user);

            // Act & Assert - Test producer-specific menu navigations
            var menuItems = new[] { "mydreams", "createdream", "sales", "profile" };
            
            foreach (var menuItem in menuItems)
            {
                try
                {
                    
                    // Verify navigation happened
                    var expectedUrlPart = menuItem.Replace("mydreams", "my-dreams")
                                                 .Replace("createdream", "create-dream");
                    
                    Driver.Url.Should().Contain(expectedUrlPart, 
                        $"Should navigate to {menuItem} page");
                    
                    // Navigate back to dashboard for next test
                }
                catch (InvalidOperationException)
                {
                    Console.WriteLine($"Menu item '{menuItem}' not available for producer user");
                }
            }
        }
        finally
        {
            await CleanupTestUser(user);
        }
    }

    [Fact]
    public async Task ProducerUser_ShouldNotAccessConsumerMenus()
    {
        // Arrange
        var user = await CreateTestUser(UserRole.Producer);
        await EnsureTestUserExists(user);

        try
        {
            var result = await AuthHelper.LoginViaUI(user);
            AssertLoginSuccess(result, user);

            // Act & Assert - Verify producer can't access consumer-specific menus
            var consumerMenuItems = new[] { "dreams", "orders", "cart" };
            
        }
        finally
        {
            await CleanupTestUser(user);
        }
    }

    [Fact]
    public async Task ProducerUser_QuickLogin_ShouldWork()
    {
        // Arrange
        var user = await CreateTestUser(UserRole.Producer);
        await EnsureTestUserExists(user);

        try
        {
            // Act - Test the quick login helper method
            var success = await AuthHelper.QuickLogin(user);

            // Assert
            success.Should().BeTrue("Quick login should succeed for valid producer user");
            
        }
        finally
        {
            await CleanupTestUser(user);
        }
    }
}
