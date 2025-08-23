using Tests.Autotests.Configuration;
using Tests.Autotests.Models;
using System.Net.Http;
using System.Text;
using FluentAssertions;

namespace Tests.Autotests.Helpers;

public class ApiHelper : IDisposable
{
    private readonly HttpClient _httpClient;

    public ApiHelper()
    {
        Console.WriteLine($"[DEBUG] Creating ApiHelper with base URL: {TestConfiguration.IdentityApiUrl}");
        
        _httpClient = new HttpClient()
        {
            BaseAddress = new Uri(TestConfiguration.IdentityApiUrl)
        };
        
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    public async Task<bool> RegisterUserViaApiAsync(TestUser user)
    {
        try
        {
            var registerData = new
            {
                email = user.Email,
                password = user.Password,
                passwordRepeat = user.Password,
                name = user.Name
            };

            var json = JsonSerializer.Serialize(registerData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            string endpoint = user.Role switch
            {
                UserRole.Consumer => "ConsumerAuth",
                UserRole.Producer => "ProducerAuth", 
                _ => throw new NotSupportedException($"Registration not supported for role: {user.Role}")
            };

            var fullUrl = new Uri(_httpClient.BaseAddress!, endpoint).ToString();
            Console.WriteLine($"[DEBUG] Attempting to register user at: {fullUrl}");
            
            var response = await _httpClient.PutAsync(endpoint, content);
            
            Console.WriteLine($"[DEBUG] Response status: {response.StatusCode}");
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[DEBUG] Response content: {errorContent}");
            }
            
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error registering user via API: {ex.Message}");
            return false;
        }
    }

    public async Task<string?> LoginUserViaApiAsync(TestUser user)
    {
        try
        {
            var loginData = new
            {
                email = user.Email,
                password = user.Password
            };

            var json = JsonSerializer.Serialize(loginData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            string endpoint = user.Role switch
            {
                UserRole.Consumer => "ConsumerAuth",
                UserRole.Producer => "ProducerAuth",
                _ => throw new NotSupportedException($"Login not supported for role: {user.Role}")
            };

            var fullUrl = new Uri(_httpClient.BaseAddress!, endpoint).ToString();
            Console.WriteLine($"[DEBUG] Attempting to login user at: {fullUrl}");

            var response = await _httpClient.PostAsync(endpoint, content);
            
            Console.WriteLine($"[DEBUG] Response status: {response.StatusCode}");
            
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var authResponse = JsonSerializer.Deserialize<AuthResponse>(responseContent);
                return authResponse?.AccessToken;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[DEBUG] Login failed. Response content: {errorContent}");
            }

            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error logging in user via API: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> DeleteUserViaApiAsync(TestUser user, string accessToken)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            string endpoint = user.Role switch
            {
                UserRole.Consumer => "ConsumerUser",
                _ => throw new NotSupportedException($"Deletion not supported for role: {user.Role}")
            };

            var response = await _httpClient.DeleteAsync(endpoint);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting user via API: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> IsServiceHealthyAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("health");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }

    private class AuthResponse
    {
        public string? AccessToken { get; set; }
        public object? UserData { get; set; }
    }
}
