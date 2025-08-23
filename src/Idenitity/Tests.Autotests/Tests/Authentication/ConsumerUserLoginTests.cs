using Tests.Autotests.TestBase;
using Tests.Autotests.Models;
using Tests.Autotests.PageObjects.Authentication;
using FluentAssertions;

namespace Tests.Autotests.Tests.Authentication;

public class ConsumerUserLoginTests : BaseLoginTest
{
    [Fact]
    public async Task ConsumerUser_ValidCredentials_ShouldLoginSuccessfully()
    {
        // Arrange
        var user = await CreateTestUser(UserRole.Consumer);
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
    public async Task ConsumerUser_InvalidPassword_ShouldFailLogin()
    {
        // Arrange
        var user = await CreateTestUser(UserRole.Consumer);
        await EnsureTestUserExists(user);

        var invalidUser = new TestUser
        {
            Email = user.Email,
            Password = "InvalidPassword123!",
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
    public async Task ConsumerUser_InvalidEmail_ShouldFailLogin()
    {
        // Arrange
        var invalidUser = TestUserFactory.CreateConsumerUser(
            email: "nonexistent@test.com",
            password: "TestPassword123!");

        // Act
        var result = await AuthHelper.LoginViaUI(invalidUser);

        // Assert
        AssertLoginFailure(result, "email");
    }

    [Fact]
    public async Task ConsumerUser_EmptyCredentials_ShouldShowValidationErrors()
    {
        // Arrange
        var loginPage = new LoginPage(Driver);
        loginPage.NavigateToPage();

        // Act - Try to login with empty fields
        loginPage.ClickLoginButton();

        // Assert
        loginPage.IsOnLoginPage().Should().BeTrue("Should remain on login page");

        // Note: Validation error checking would depend on the actual UI implementation
        // This test structure allows for easy extension once the UI is available
    }

    [Fact]
    public async Task ConsumerUser_NavigateToRegistrationPage_ShouldWork()
    {
        // Arrange
        var loginPage = new LoginPage(Driver);
        loginPage.NavigateToPage();

        // Act
        var registerPage = loginPage.ClickRegisterLink();

        // Assert
        Driver.Url.Should().Contain("register", "Should navigate to registration page");
    }

    [Fact]
    public async Task ConsumerUser_LoginAfterRegistration_ShouldWork()
    {
        // Arrange
        var user = await CreateTestUser(UserRole.Consumer);
        
        // Register via UI
        var registerPage = new RegisterPage(Driver);
        registerPage.NavigateToPage();
        registerPage.RegisterUser(user);

        // Wait a bit for registration to complete
        await Task.Delay(2000);

        try
        {
            // Act - Login after registration
            var result = await AuthHelper.LoginViaUI(user);

            // Assert
            AssertLoginSuccess(result, user);
        }
        finally
        {
            await CleanupTestUser(user);
        }
    }
}
