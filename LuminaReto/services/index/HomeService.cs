using System.Text.Json;
using LuminaReto.Models;

public class HomeService : IHomeService
{
    private readonly HttpClient _httpClient;
    private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public HomeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<DashboardData> GetDashboard(int idUsuario)
    {
        var url = "http://localhost:4999/dashboard/" + idUsuario;
        var data = await _httpClient.GetFromJsonAsync<DashboardData>(url, _jsonOptions);
        return data ?? new DashboardData();
    }
}