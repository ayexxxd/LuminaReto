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
    var url = "https://10.22.227.188:4999/dashboard/" + idUsuario;

    var response = await _httpClient.GetAsync(url);

    var body = await response.Content.ReadAsStringAsync();

    Console.WriteLine("RESPONSE:");
    Console.WriteLine(body);

    response.EnsureSuccessStatusCode();

    return JsonSerializer.Deserialize<DashboardData>(body, _jsonOptions)
           ?? new DashboardData();
}}