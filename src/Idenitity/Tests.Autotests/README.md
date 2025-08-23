# DreamsShop Identity Service - Automated Tests

This project contains Selenium-based automated tests for the DreamsShop Identity service, focusing on login functionality and user navigation.

## 🏗️ Architecture

The test framework is built with scalability and maintainability in mind:

### 📁 Project Structure
```
Tests.Autotests/
├── Configuration/          # Test configuration and settings
├── Core/                  # Base test classes and WebDriver setup
├── Helpers/               # API helpers and authentication utilities
├── Models/                # Test data models and user factories
├── PageObjects/           # Page Object Model implementation
│   ├── Authentication/    # Login, registration, forgot password pages
│   ├── Dashboard/         # Dashboard and main application pages
│   └── Navigation/        # Navigation menu and routing
├── TestBase/              # Base test classes with common functionality
└── Tests/                 # Actual test implementations
    ├── Authentication/    # Login/authentication tests
    └── Infrastructure/    # Framework and setup tests
```

### 🎯 Key Features

- **Multi-User Support**: Easily test Consumer, Producer, and Admin users
- **Cross-Browser Testing**: Firefox and Chrome support
- **Page Object Model**: Maintainable and reusable page interactions
- **API Integration**: Test data setup/cleanup via API calls
- **Scalable Navigation**: Role-based menu testing
- **Headless Support**: For CI/CD environments
- **Screenshot Capture**: Automatic failure documentation
- **FluentAssertions**: Readable test assertions

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK
- Firefox and/or Chrome browsers installed
- DreamsShop services running (Identity service on port 5001)

### Running Tests

#### Basic Test Execution
```bash
# Run all tests
dotnet test

# Run specific test class
dotnet test --filter "ConsumerUserLoginTests"

# Run with verbose output
dotnet test --verbosity normal
```

#### Browser Configuration
```bash
# Run with Chrome (default is Firefox)
BROWSER=chrome dotnet test

# Run in headless mode (good for CI/CD)
HEADLESS=true dotnet test

# Combine options
BROWSER=chrome HEADLESS=true dotnet test
```

#### Test Categories
```bash
# Run only authentication tests
dotnet test --filter "Category=Authentication"

# Run infrastructure tests (connectivity, setup)
dotnet test --filter "TestInfrastructureTests"
```

## 🧪 Test Types

### Consumer User Tests (`ConsumerUserLoginTests`)
- Valid credential login
- Invalid credential handling
- Registration flow
- Consumer-specific navigation
- Menu access verification
- Logout functionality

### Producer User Tests (`ProducerUserLoginTests`)
- Producer login scenarios  
- Producer-specific navigation
- Role-based access control
- Cross-role access prevention

### Multi-User Tests (`MultiUserLoginTests`)
- User type switching
- Bulk user operations
- Parameterized role testing
- Menu navigation across roles

### Infrastructure Tests (`TestInfrastructureTests`)
- Framework setup verification
- Service connectivity
- Configuration validation
- Browser compatibility

## 🔧 Configuration

### Test Settings (`TestConfiguration.cs`)
```csharp
// Service URLs
BaseUrl = "http://localhost:80"
IdentityApiUrl = "http://localhost:5001/api"

// Test users (modify for your environment)
ConsumerUser.Email = "consumer.test@example.com"
ProducerUser.Email = "producer.test@example.com"
```

### Environment Variables
- `BROWSER`: "firefox" or "chrome" (default: firefox)
- `HEADLESS`: "true" or "false" (default: false)

## 🏃‍♂️ Writing New Tests

### Adding a New User Type
1. Update `UserRole` enum in `Models/TestUser.cs`
2. Add factory method in `TestUserFactory`
3. Update navigation menus in `NavigationMenu.cs`
4. Create new test class inheriting from `BaseLoginTest`

### Adding New Page Objects
1. Create page class inheriting from `BasePage`
2. Define element locators using `By` selectors
3. Implement page actions as fluent methods
4. Add page verification methods

### Example: Adding Admin Tests
```csharp
public class AdminUserLoginTests : BaseLoginTest
{
    [Fact]
    public async Task AdminUser_ValidCredentials_ShouldLoginSuccessfully()
    {
        var user = await CreateTestUser(UserRole.Admin);
        await EnsureTestUserExists(user);

        var result = await AuthHelper.LoginViaUI(user);
        
        AssertLoginSuccess(result, user);
        result.DashboardPage.VerifyUserRoleAccess(UserRole.Admin)
            .Should().BeTrue();
    }
}
```

## 🛠️ Extending Navigation

### Adding New Menu Items
1. Update `NavigationMenu.cs` with new element locators
2. Add navigation methods for the new menu
3. Update role-based navigation methods
4. Add tests to verify menu accessibility

### Example: Adding Settings Menu
```csharp
// In NavigationMenu.cs
private readonly By _settingsLink = By.CssSelector("a[href='/settings']");

public void ClickSettings()
{
    ClickElement(_settingsLink);
}

// Update NavigateBasedOnRole method to include settings
```

## 🚦 Best Practices

### Test Data Management
- Use unique timestamps for test user emails
- Always cleanup test data in `finally` blocks
- Prefer API setup over UI setup for speed
- Use factory methods for consistent test data

### Page Object Design
- Use fluent interfaces for chaining actions
- Implement both specific and generic element locators
- Add verification methods to page objects
- Keep page objects focused on single responsibility

### Test Organization
- Group related tests in single test classes
- Use descriptive test names following Given_When_Then
- Use `Theory` and `InlineData` for parameterized tests
- Add comments explaining complex test scenarios

### Error Handling
- Always dispose of resources properly
- Use try-finally blocks for cleanup
- Take screenshots on failures
- Log meaningful error messages

## 🔍 Troubleshooting

### Common Issues
1. **Service not running**: Ensure Identity service is running on port 5001
2. **Browser not found**: Install Firefox/Chrome or check PATH
3. **Timeout errors**: Increase timeout values in configuration
4. **Element not found**: Update locators or add wait conditions

### Debug Tips
- Run tests with `--verbosity normal` for detailed output
- Use `TakeScreenshot()` method for visual debugging
- Check `Screenshots/` folder for failure images
- Enable browser UI (set `HEADLESS=false`) for visual debugging

## 📈 Scaling the Framework

This framework is designed to scale with your application:

- **Add new user roles** by extending the UserRole enum and factories
- **Add new pages** by creating new PageObject classes
- **Add new test scenarios** by creating new test classes
- **Add new browsers** by extending the WebDriver factory
- **Add new environments** by updating configuration

The modular design ensures that changes in one area don't break existing tests, making it easy to maintain and extend as your application grows.
