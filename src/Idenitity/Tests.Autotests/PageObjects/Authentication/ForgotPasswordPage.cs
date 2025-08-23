using Tests.Autotests.Configuration;

namespace Tests.Autotests.PageObjects.Authentication;

public class ForgotPasswordPage : BasePage
{
    private readonly By _emailField = By.Id("email");
    private readonly By _sendButton = By.Id("sendButton");
    private readonly By _backToLoginLink = By.Id("backToLoginLink");
    private readonly By _errorMessage = By.CssSelector(".error-message");
    private readonly By _successMessage = By.CssSelector(".success-message");

    protected override string PageUrl => $"{TestConfiguration.BaseUrl}/forgot-password";

    public ForgotPasswordPage(IWebDriver driver) : base(driver) { }

    public ForgotPasswordPage EnterEmail(string email)
    {
        EnterText(_emailField, email);
        return this;
    }

    public ForgotPasswordPage ClickSendButton()
    {
        ClickElement(_sendButton);
        return this;
    }

    public bool IsSuccessful()
    {
        return IsElementPresent(_successMessage);
    }

    public bool HasError()
    {
        return IsElementPresent(_errorMessage);
    }

    public string GetErrorMessage()
    {
        return HasError() ? GetText(_errorMessage) : string.Empty;
    }

    public LoginPage ClickBackToLoginLink()
    {
        ClickElement(_backToLoginLink);
        return new LoginPage(Driver);
    }
}
