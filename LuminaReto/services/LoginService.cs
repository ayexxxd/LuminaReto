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
        var url = "https://127.0.0.1:5010/login/" + email + "/" + password;

        var response = await _httpClient.GetAsync(url);
        var reponseJson = await response.Content.ReadAsStringAsync();

        return int.Parse(reponseJson);
    }
}
