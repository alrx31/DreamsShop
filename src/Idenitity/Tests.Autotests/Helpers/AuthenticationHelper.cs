using Tests.Autotests.PageObjects.Authentication;

namespace Tests.Autotests.Helpers;

public class AuthenticationHelper
{
    private readonly IWebDriver _driver;
    private readonly ApiHelper _apiHelper;

    public AuthenticationHelper(IWebDriver driver)
    {
        _driver = driver;
        _apiHelper = new ApiHelper();
    }

    /// <summary>
    /// Performs complete login flow via UI
    /// </summary>
    public async Task<LoginResult> LoginViaUI(TestUser user)
    {
        var loginPage = new LoginPage(_driver);
        loginPage.NavigateToPage();

        if (!loginPage.IsOnLoginPage())
        {
            return new LoginResult 
            { 
                Success = false, 
                ErrorMessage = "Could not navigate to login page" 
            };
        }

        loginPage.LoginAs(user);
        loginPage.WaitForLoginToComplete();

        if (loginPage.HasLoginError())
        {
            return new LoginResult
            {
                Success = false,
                ErrorMessage = loginPage.GetErrorMessage()
            };
        }

        if (loginPage.IsLoginSuccessful())
        {
            return new LoginResult
            {
                Success = true,
            };
        }

        return new LoginResult
        {
            Success = false,
            ErrorMessage = "Login did not complete successfully"
        };
    }

    /// <summary>
    /// Ensures user exists (creates if needed) and performs login
    /// </summary>
    public async Task<LoginResult> EnsureUserAndLogin(TestUser user)
    {
        // Try to register user first (it's ok if it fails due to existing user)
        await _apiHelper.RegisterUserViaApiAsync(user);

        // Now attempt login
        return await LoginViaUI(user);
    }

    /// <summary>
    /// Quick login method for testing - tries API first, falls back to UI
    /// </summary>
    public async Task<bool> QuickLogin(TestUser user)
    {
        try
        {
            // Try API login first for speed
            var token = await _apiHelper.LoginUserViaApiAsync(user);
            if (!string.IsNullOrEmpty(token))
            {
                // If API login successful, navigate to dashboard
               
                return true;
            }

            // Fallback to UI login
            var result = await LoginViaUI(user);
            return result.Success;
        }
        catch
        {
            return false;
        }
    }

    public void Dispose()
    {
        _apiHelper?.Dispose();
    }
}

public class LoginResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
}
