using System.Text.Json;
using LuminaReto.Models;

public class LoginService : ILoginService
{
    private readonly HttpClient _httpClient;
    public LoginService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<int> Login(string email, string password)
    {
        var url = "https://127.0.0.1:5010/login";

        var body = new
        {
            email,
            password
        };

        var response = await _httpClient.PostAsJsonAsync(url, body);
        var responseJson = await response.Content.ReadAsStringAsync();

        return int.Parse(responseJson);
    }
}