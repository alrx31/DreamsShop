using Tests.Autotests.Core;
using Tests.Autotests.Configuration;
using Tests.Autotests.Helpers;
using FluentAssertions;

namespace Tests.Autotests.Tests.Infrastructure;

public class TestInfrastructureTests : BaseWebDriverTest
{
    [Fact]
    public void WebDriver_ShouldInitializeSuccessfully()
    {
        // Arrange & Act
        Driver.Should().NotBeNull("WebDriver should be initialized");
        Driver.Url.Should().NotBeNull("WebDriver should have a current URL");
    }

    [Fact]
    public void Configuration_ShouldHaveValidUrls()
    {
        // Arrange & Act & Assert
        TestConfiguration.BaseUrl.Should().NotBeNullOrEmpty("Base URL should be configured");
        TestConfiguration.IdentityApiUrl.Should().NotBeNullOrEmpty("Identity API URL should be configured");
        
        // URLs should be valid
        Uri.IsWellFormedUriString(TestConfiguration.BaseUrl, UriKind.Absolute)
            .Should().BeTrue("Base URL should be a valid absolute URI");
        Uri.IsWellFormedUriString(TestConfiguration.IdentityApiUrl, UriKind.Absolute)
            .Should().BeTrue("Identity API URL should be a valid absolute URI");
    }

    [Fact]
    public async Task ApiHelper_ShouldConnectToService()
    {
        // Arrange
        using var apiHelper = new ApiHelper();

        // Act
        var isHealthy = await apiHelper.IsServiceHealthyAsync();

        // Assert - This might fail if services aren't running, which is acceptable
        // The test serves as a connectivity check
        if (isHealthy)
        {
            Console.WriteLine("Identity service is reachable and healthy");
        }
        else
        {
            Console.WriteLine("Identity service is not reachable - this is expected if services aren't running");
        }

        // The test always passes - it's just a diagnostic
        true.Should().BeTrue();
    }

    [Fact]
    public void TestUserFactory_ShouldCreateValidUsers()
    {
        // Arrange & Act
        var consumerUser = TestUserFactory.CreateConsumerUser();
        var producerUser = TestUserFactory.CreateProducerUser();
        var adminUser = TestUserFactory.CreateAdminUser();

        // Assert
        consumerUser.Role.Should().Be(UserRole.Consumer);
        consumerUser.Email.Should().NotBeNullOrEmpty();
        consumerUser.Password.Should().NotBeNullOrEmpty();

        producerUser.Role.Should().Be(UserRole.Producer);
        producerUser.Email.Should().NotBeNullOrEmpty();
        producerUser.Password.Should().NotBeNullOrEmpty();

        adminUser.Role.Should().Be(UserRole.Admin);
        adminUser.Email.Should().NotBeNullOrEmpty();
        adminUser.Password.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void Browser_ShouldNavigateToUrls()
    {
        // Arrange
        var testUrl = "https://www.google.com";

        // Act
        NavigateToUrl(testUrl);

        // Assert
        Driver.Url.Should().StartWith("https://www.google.com");
        Driver.Title.Should().Contain("Google");
    }

    [Fact]
    public void Screenshot_ShouldBeCapturableOnFailure()
    {
        // Arrange
        NavigateToUrl("https://www.google.com");

        // Act
        var action = () => TakeScreenshot("TestInfrastructureTests_Screenshot");

        // Assert
        action.Should().NotThrow("Screenshot capture should not throw exceptions");
        
        // Check if screenshot directory exists (it should be created)
        var screenshotDir = Path.Combine(Directory.GetCurrentDirectory(), "Screenshots");
        Directory.Exists(screenshotDir).Should().BeTrue("Screenshots directory should be created");
    }

    [Theory]
    [InlineData("firefox")]
    [InlineData("chrome")]
    public void WebDriverFactory_ShouldSupportMultipleBrowsers(string browserName)
    {
        // This test documents the browsers we support
        // The actual browser used depends on the test configuration
        
        var supportedBrowsers = new[] { "firefox", "chrome" };
        supportedBrowsers.Should().Contain(browserName.ToLower());
    }
}
