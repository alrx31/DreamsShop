namespace Tests.Autotests.Configuration;

public static class TestConfiguration
{
    public static string BaseUrl => "http://localhost";
    public static string IdentityApiUrl => "http://localhost/identity/api/";
    public static string BusinessApiUrl => "http://localhost/business/api/";

    public static int DefaultTimeoutSeconds => 10;
    public static int LongTimeoutSeconds => 30;
    
    public static bool IsHeadless => Environment.GetEnvironmentVariable("HEADLESS")?.ToLower() == "true";
    public static string Browser => Environment.GetEnvironmentVariable("BROWSER") ?? "firefox";
    
    // Test user credentials - in real scenarios, these should come from secure config
    public static class TestUsers
    {
        public static class ConsumerUser
        {
            public static string Email => "consumer.test@example.com";
            public static string Password => "TestPassword123!";
        }
        
        public static class ProducerUser
        {
            public static string Email => "producer.test@example.com";
            public static string Password => "TestPassword123!";
        }
        
        public static class AdminUser
        {
            public static string Email => "admin.test@example.com";
            public static string Password => "AdminPassword123!";
        }
    }
}
