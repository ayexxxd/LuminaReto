using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using LuminaReto.Models.Formularios.Dtos;

namespace LuminaReto.Services.Formularios;

public class FormularioService : IFormularioService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _json = new() 
    { 
        PropertyNameCaseInsensitive = true,
        
        NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
    };
    public FormularioService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<PreguntaDto>> ObtenerPreguntasAsync(int idFormulario)
    {
        try
        {
            var res = await _httpClient.GetAsync($"/PF/formularios/{idFormulario}/preguntas");
            if (!res.IsSuccessStatusCode) return new();
            return JsonSerializer.Deserialize<List<PreguntaDto>>(
                await res.Content.ReadAsStringAsync(), _json) ?? new();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ObtenerPreguntasAsync: {ex.Message}");
            return new();
        }
    }

    public async Task<List<OpcionDto>> ObtenerOpcionesAsync(int idPregunta)
    {
        try
        {
            var res = await _httpClient.GetAsync($"/PF/formularios/preguntas/{idPregunta}/opciones");
            if (!res.IsSuccessStatusCode) return new();
            return JsonSerializer.Deserialize<List<OpcionDto>>(
                await res.Content.ReadAsStringAsync(), _json) ?? new();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ObtenerOpcionesAsync: {ex.Message}");
            return new();
        }
    }

    public async Task<List<FormularioDisponibleDto>> ObtenerFormulariosDisponiblesAsync(int idUsuario)
    {
        try
        {
            var res = await _httpClient.GetAsync($"/PF/usuarios/{idUsuario}/formularios-disponibles");
            if (!res.IsSuccessStatusCode) return new();
            return JsonSerializer.Deserialize<List<FormularioDisponibleDto>>(
                await res.Content.ReadAsStringAsync(), _json) ?? new();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ObtenerFormulariosDisponiblesAsync: {ex.Message}");
            return new();
        }
    }

    public async Task<List<FormularioCompletadoDto>> ObtenerFormulariosCompletadosAsync(int idUsuario)
    {
        try
        {
            var res = await _httpClient.GetAsync($"/PF/usuarios/{idUsuario}/formularios-completados");
            if (!res.IsSuccessStatusCode) return new();
            return JsonSerializer.Deserialize<List<FormularioCompletadoDto>>(
                await res.Content.ReadAsStringAsync(), _json) ?? new();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ObtenerFormulariosCompletadosAsync: {ex.Message}");
            return new();
        }
    }

    public async Task<ProgresoDto> ObtenerProgresoAsync(int idUsuario)
    {
        try
        {
            var res = await _httpClient.GetAsync($"/PF/usuarios/{idUsuario}/progreso");
            if (!res.IsSuccessStatusCode) return new();
            
            
            return JsonSerializer.Deserialize<ProgresoDto>(
                await res.Content.ReadAsStringAsync(), _json) ?? new();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ObtenerProgresoAsync: {ex.Message}");
            return new();
        }
    }

    public async Task CompletarFormularioAsync(int idUsuario, int idFormulario)
    {
        try
        {
            var payload = JsonSerializer.Serialize(new { idUsuario, idFormulario });
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            await _httpClient.PostAsync("/PF/completar-formulario", content);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"CompletarFormularioAsync: {ex.Message}");
        }
    }
}