using LuminaReto.Models;
using System.Text.Json;

public class ClasificacionService : IClasificacionService
{
    private readonly HttpClient _httpClient;
    private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public ClasificacionService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<RankingResponseDto?> GetRanking(int idUsuario, int limite = 20)
    {
        var response = await _httpClient.GetAsync($"/ranking?id_usuario={idUsuario}&limite={limite}");
        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<RankingResponseDto>(json, _jsonOptions);
    }
}
