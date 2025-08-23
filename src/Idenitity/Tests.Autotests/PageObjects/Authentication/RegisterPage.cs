using Tests.Autotests.Configuration;
using Tests.Autotests.Models;

namespace Tests.Autotests.PageObjects.Authentication;

public class RegisterPage : BasePage
{
    // Page elements
    private readonly By _emailField = By.Id("email");
    private readonly By _passwordField = By.Id("password");
    private readonly By _confirmPasswordField = By.Id("confirmPassword");
    private readonly By _firstNameField = By.Id("firstName");
    private readonly By _lastNameField = By.Id("lastName");
    private readonly By _registerButton = By.Id("registerButton");
    private readonly By _loginLink = By.Id("loginLink");
    private readonly By _errorMessage = By.CssSelector(".error-message");
    private readonly By _successMessage = By.CssSelector(".success-message");
    
    // Alternative selectors
    private readonly By _emailFieldAlt = By.CssSelector("input[type='email'], input[name='email']");
    private readonly By _passwordFieldAlt = By.CssSelector("input[type='password'][name='password']");
    private readonly By _confirmPasswordFieldAlt = By.CssSelector("input[type='password'][name='confirmPassword'], input[type='password'][name='confirm']");

    protected override string PageUrl => $"{TestConfiguration.BaseUrl}/register";

    public RegisterPage(IWebDriver driver) : base(driver) { }

    public RegisterPage EnterEmail(string email)
    {
        try
        {
            EnterText(_emailField, email);
        }
        catch (NoSuchElementException)
        {
            EnterText(_emailFieldAlt, email);
        }
        return this;
    }

    public RegisterPage EnterPassword(string password)
    {
        try
        {
            EnterText(_passwordField, password);
        }
        catch (NoSuchElementException)
        {
            EnterText(_passwordFieldAlt, password);
        }
        return this;
    }

    public RegisterPage EnterConfirmPassword(string confirmPassword)
    {
        try
        {
            EnterText(_confirmPasswordField, confirmPassword);
        }
        catch (NoSuchElementException)
        {
            EnterText(_confirmPasswordFieldAlt, confirmPassword);
        }
        return this;
    }

    public RegisterPage EnterFirstName(string firstName)
    {
        if (IsElementPresent(_firstNameField))
        {
            EnterText(_firstNameField, firstName);
        }
        return this;
    }

    public RegisterPage EnterLastName(string lastName)
    {
        if (IsElementPresent(_lastNameField))
        {
            EnterText(_lastNameField, lastName);
        }
        return this;
    }

    public RegisterPage ClickRegisterButton()
    {
        ClickElement(_registerButton);
        return this;
    }

    public RegisterPage RegisterUser(TestUser user)
    {
        EnterEmail(user.Email);
        EnterPassword(user.Password);
        EnterConfirmPassword(user.Password);
        
        if (!string.IsNullOrEmpty(user.Name))
            EnterFirstName(user.Name);

        return ClickRegisterButton();
    }

    public bool IsRegistrationSuccessful()
    {
        return IsElementPresent(_successMessage) || !Driver.Url.Contains("/register");
    }

    public bool HasRegistrationError()
    {
        return IsElementPresent(_errorMessage);
    }

    public string GetErrorMessage()
    {
        return HasRegistrationError() ? GetText(_errorMessage) : string.Empty;
    }

    public LoginPage ClickLoginLink()
    {
        ClickElement(_loginLink);
        return new LoginPage(Driver);
    }
}
