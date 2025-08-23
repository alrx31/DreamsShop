using Tests.Autotests.Configuration;

namespace Tests.Autotests.Core;

public abstract class BaseWebDriverTest : IDisposable
{
    protected readonly IWebDriver Driver;
    protected readonly WebDriverWait Wait;
    
    protected BaseWebDriverTest()
    {
        Driver = CreateWebDriver();
        Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(TestConfiguration.DefaultTimeoutSeconds));
        Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(TestConfiguration.DefaultTimeoutSeconds);
    }

    private IWebDriver CreateWebDriver()
    {
        return TestConfiguration.Browser.ToLower() switch
        {
            "chrome" => CreateChromeDriver(),
            "firefox" => CreateFirefoxDriver(),
            _ => CreateFirefoxDriver()
        };
    }

    private IWebDriver CreateFirefoxDriver()
    {
        var options = new FirefoxOptions();
        
        if (TestConfiguration.IsHeadless)
        {
            options.AddArgument("--headless");
        }
        
        //options.AddArgument("--headless");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");
        options.AddArgument("--disable-gpu");
        options.AddArgument("--window-size=1920,1080");
        
        return new FirefoxDriver(options);
    }

    private IWebDriver CreateChromeDriver()
    {
        var options = new ChromeOptions();
        
        if (TestConfiguration.IsHeadless)
        {
            options.AddArgument("--headless");
        }
        
        //options.AddArgument("--headless");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");
        options.AddArgument("--disable-gpu");
        options.AddArgument("--window-size=1920,1080");
        
        return new ChromeDriver(options);
    }

    protected void NavigateToUrl(string url)
    {
        Driver.Navigate().GoToUrl(url);
    }

    protected void WaitForElement(By locator, int timeoutSeconds = 0)
    {
        var timeout = timeoutSeconds > 0 ? timeoutSeconds : TestConfiguration.DefaultTimeoutSeconds;
        var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeout));
        wait.Until(d => d.FindElement(locator));
    }

    protected void WaitForElementToBeClickable(By locator, int timeoutSeconds = 0)
    {
        var timeout = timeoutSeconds > 0 ? timeoutSeconds : TestConfiguration.DefaultTimeoutSeconds;
        var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeout));
        wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator));
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

    protected void TakeScreenshot(string testName)
    {
        try
        {
            var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
            var fileName = $"{testName}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            var directory = Path.Combine(Directory.GetCurrentDirectory(), "Screenshots");
            
            Directory.CreateDirectory(directory);
            var filePath = Path.Combine(directory, fileName);
            
            screenshot.SaveAsFile(filePath);
            Console.WriteLine($"Screenshot saved: {filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to take screenshot: {ex.Message}");
        }
    }

    public virtual void Dispose()
    {
        try
        {
            Driver?.Quit();
            Driver?.Dispose();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during WebDriver cleanup: {ex.Message}");
        }
    }
}
