using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using LuminaReto.Models;
using LuminaReto.Services;

public class PerfilService : IPerfilService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "https://10.22.188.150:5001"; // Asegúrate que coincida con tu API

    public PerfilService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PerfilUsuario> GetPerfil(int idUsuario)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<PerfilUsuario>($"{_baseUrl}/perfil/obtenerPerfil/{idUsuario}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener perfil: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> EditarPerfil(int idUsuario, string nombre, string correo)
    {
        try
        {
            var body = new { id_usuario = idUsuario, nombre = nombre, correo = correo };
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/usuario/editar", body);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al editar perfil: {ex.Message}");
            return false;
        }
    }

    public async Task<EstadisticasUsuario> GetEstadisticas(int idUsuario)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<EstadisticasUsuario>($"{_baseUrl}/perfil/obtenerEstadisticas/{idUsuario}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener estadísticas: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> GuardarPerfil(PerfilUsuario perfil)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/perfil/guardarPerfil", perfil);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al guardar perfil: {ex.Message}");
            return false;
        }
    }
}