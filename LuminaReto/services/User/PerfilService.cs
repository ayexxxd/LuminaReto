using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using LuminaReto.Models;      // Donde viven PerfilUsuario y EstadisticasUsuario
using LuminaReto.Services;    // Donde vive la interfaz IPerfilService

public class PerfilService : IPerfilService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "https://10.22.188.150:5001"; 

    public PerfilService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PerfilUsuario> GetPerfil(int idUsuario)
    {
        try
        {
            var url = _baseUrl + "/perfil/obtenerPerfil/" + idUsuario;
            return await _httpClient.GetFromJsonAsync<PerfilUsuario>(url);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al obtener perfil: " + ex.Message);
            return null;
        }
    }

    public async Task<bool> EditarPerfil(int idUsuario, string nombre, string correo)
    {
        try
        {
            var url = _baseUrl + "/api/usuario/editar";
            var body = new { id_usuario = idUsuario, nombre = nombre, correo = correo };
            
            var response = await _httpClient.PostAsJsonAsync(url, body);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al editar perfil: " + ex.Message);
            return false;
        }
    }

    public async Task<EstadisticasUsuario> GetEstadisticas(int idUsuario)
    {
        try
        {
            var url = _baseUrl + "/perfil/obtenerEstadisticas/" + idUsuario;
            return await _httpClient.GetFromJsonAsync<EstadisticasUsuario>(url);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al obtener estadísticas: " + ex.Message);
            return null;
        }
    }

    public async Task<bool> GuardarPerfil(PerfilUsuario perfil)
    {
        try
        {
            var url = _baseUrl + "/perfil/guardarPerfil";
            var response = await _httpClient.PostAsJsonAsync(url, perfil);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al guardar perfil: " + ex.Message);
            return false;
        }
    }
}