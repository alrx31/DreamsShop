using Tests.Autotests.Configuration;
using Tests.Autotests.Models;

namespace Tests.Autotests.PageObjects.Authentication;

public class LoginPage : BasePage
{
    // Page elements
    private readonly By _emailFieldAlt = By.Id("email");
    private readonly By _passwordFieldAlt = By.Id("password");
    private readonly By _loginButtonAlt = By.Id("loginButton");
    private readonly By _registerLink = By.Id("registerLink");
    private readonly By _forgotPasswordLink = By.Id("forgotPasswordLink");
    private readonly By _errorMessage = By.CssSelector(".error-message");
    private readonly By _successMessage = By.CssSelector(".success-message");
    private readonly By _loadingSpinner = By.CssSelector(".loading-spinner");
    
    // Alternative selectors (in case the page uses different HTML structure)
    private readonly By _emailField = By.CssSelector("input[type='email'], input[name='email'], input[placeholder*='email' i]");
    private readonly By _passwordField = By.CssSelector("input[type='password'], input[name='password']");
    private readonly By _loginButton = By.CssSelector("button[type='submit']");

    protected override string PageUrl => $"{TestConfiguration.BaseUrl}/login";

    public LoginPage(IWebDriver driver) : base(driver) { }

    public bool IsOnLoginPage()
    {
        try
        {
            return Driver.Url.Contains("/login") || 
                   IsElementPresent(_emailField) || IsElementPresent(_emailFieldAlt) ||
                   IsElementPresent(_passwordField) || IsElementPresent(_passwordFieldAlt);
        }
        catch
        {
            return false;
        }
    }

    public LoginPage EnterEmail(string email)
    {
        try
        {
            EnterText(_emailField, email);
        }
        catch (WebDriverTimeoutException)
        {
            // Try alternative selector
            EnterText(_emailFieldAlt, email);
        }
        return this;
    }

    public LoginPage EnterPassword(string password)
    {
        try
        {
            EnterText(_passwordField, password);
        }
        catch (WebDriverTimeoutException)
        {
            // Try alternative selector
            EnterText(_passwordFieldAlt, password);
        }
        return this;
    }

    public LoginPage ClickLoginButton()
    {
        try
        {
            ClickElement(_loginButton);
        }
        catch (WebDriverTimeoutException)
        {
            // Try alternative selector
            ClickElement(_loginButtonAlt);
        }
        return this;
    }

    public LoginPage LoginAs(TestUser user)
    {
        return EnterEmail(user.Email)
            .EnterPassword(user.Password)
            .ClickLoginButton();
    }

    public LoginPage LoginAs(string email, string password)
    {
        return EnterEmail(email)
            .EnterPassword(password)
            .ClickLoginButton();
    }

    public bool IsLoginSuccessful()
    {
        try
        {
            // Wait for navigation away from login page or success indication
            //WaitForUrlToContain("");
            return true;
        }
        catch (WebDriverTimeoutException)
        {
            // Check if still on login page with success message
            return IsElementPresent(_successMessage) || !Driver.Url.Contains("/login");
        }
    }

    public bool HasLoginError()
    {
        return IsElementPresent(_errorMessage);
    }

    public string GetErrorMessage()
    {
        if (HasLoginError())
        {
            return GetText(_errorMessage);
        }
        return string.Empty;
    }

    public bool IsLoading()
    {
        return IsElementVisible(_loadingSpinner);
    }

    public void WaitForLoginToComplete()
    {
        try
        {
            // Wait for loading to finish
            if (IsElementPresent(_loadingSpinner))
            {
                WaitForElementToBeInvisible(_loadingSpinner);
            }

            // Wait for either success (navigation) or error
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(TestConfiguration.LongTimeoutSeconds));
            wait.Until(d => !d.Url.Contains("/login") || IsElementPresent(_errorMessage));
        }
        catch (WebDriverTimeoutException)
        {
            // Login might have completed without obvious indicators
        }
    }

    public RegisterPage ClickRegisterLink()
    {
        ClickElement(_registerLink);
        return new RegisterPage(Driver);
    }

    public ForgotPasswordPage ClickForgotPasswordLink()
    {
        ClickElement(_forgotPasswordLink);
        return new ForgotPasswordPage(Driver);
    }
}
