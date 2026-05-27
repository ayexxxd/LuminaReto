using Microsoft.AspNetCore.Mvc;
using LuminaReto.Models;
using System.Text.Json;

namespace LuminaReto.Controllers;

public class ClasificacionController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ClasificacionController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Clasificacion()
    {
        var modelo = new ClasificacionViewModel();

        var idUsuario = HttpContext.Session.GetInt32("IdUsuario");
        if (idUsuario is null)
        {
            return RedirectToAction("Index", "Home");
        }

        try
        {
            var client   = _httpClientFactory.CreateClient("LuminaApi");
            var response = await client.GetAsync($"/ranking?id_usuario={idUsuario.Value}&limite=20");

            if (!response.IsSuccessStatusCode)
            {
                modelo.ErrorMessage = $"Error al obtener el ranking ({(int)response.StatusCode})";
                return View(modelo);
            }

            var json = await response.Content.ReadAsStringAsync();
            var dto = JsonSerializer.Deserialize<RankingResponseDto>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (dto is null)
            {
                modelo.ErrorMessage = "dto es null";
                return View(modelo);
            }

            var empleados = dto.Ranking.Select(r => new EmpleadoRanking
            {
                IdUsuario   = r.IdUsuario,
                Posicion    = r.Posicion,
                Nombre      = r.Nombre,
                WhirlTokens = r.WhirlTokens,
                TotalPuntos = r.TotalPuntos,
                RachaActual = r.RachaActual
            }).ToList();

            modelo.Ranking = empleados;
            modelo.Top3    = empleados.Take(3).ToList();

            modelo.UsuarioActual = new EmpleadoRanking
            {
                IdUsuario   = dto.UsuarioActual.IdUsuario,
                Posicion    = dto.UsuarioActual.Posicion,
                Nombre      = dto.UsuarioActual.Nombre,
                WhirlTokens = dto.UsuarioActual.WhirlTokens,
                TotalPuntos = dto.UsuarioActual.TotalPuntos,
                RachaActual = dto.UsuarioActual.RachaActual
            };

        }
        catch (Exception ex)
        {
            modelo.ErrorMessage = ex.Message; // temporal para debug
        }

        return View(modelo);
    }
}
