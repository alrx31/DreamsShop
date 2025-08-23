using Tests.Autotests.Configuration;

namespace Tests.Autotests.PageObjects;

public abstract class BasePage
{
    protected readonly IWebDriver Driver;
    protected readonly WebDriverWait Wait;

    protected BasePage(IWebDriver driver)
    {
        Driver = driver;
        Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(TestConfiguration.DefaultTimeoutSeconds));
    }

    protected virtual string PageUrl => string.Empty;
    
    public virtual void NavigateToPage()
    {
        if (!string.IsNullOrEmpty(PageUrl))
        {
            Driver.Navigate().GoToUrl(PageUrl);
        }
    }

    protected IWebElement FindElement(By locator)
    {
        return Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(locator));
    }

    protected IWebElement FindClickableElement(By locator)
    {
        return Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator));
    }

    protected void ClickElement(By locator)
    {
        FindClickableElement(locator).Click();
    }

    protected void EnterText(By locator, string text)
    {
        var element = FindElement(locator);
        element.Clear();
        element.SendKeys(text);
    }

    protected string GetText(By locator)
    {
        return FindElement(locator).Text;
    }

    protected bool IsElementPresent(By locator)
    {
        try
        {
            Driver.FindElement(locator);
            return true;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }

    protected bool IsElementVisible(By locator)
    {
        try
        {
            var element = Driver.FindElement(locator);
            return element.Displayed;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }

    protected void WaitForElementToBeInvisible(By locator)
    {
        Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(locator));
    }

    protected void WaitForUrlToContain(string urlPart)
    {
        Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains(urlPart));
    }

    protected void ScrollToElement(By locator)
    {
        var element = FindElement(locator);
        ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
        Thread.Sleep(500); // Small delay for smooth scrolling
    }
}
