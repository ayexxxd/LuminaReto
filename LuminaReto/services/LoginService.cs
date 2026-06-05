using System.Text.Json;
using LuminaReto.Models;

public class LoginService : ILoginService
{
    private readonly HttpClient _httpClient;
    public LoginService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task <int> Login(string email, string password)
    {
        var url = "https://10.14.255.45:5010/login/" + email + "/" + password;
        
        return _httpClient.GetFromJsonAsync<int>(url);
    }
}