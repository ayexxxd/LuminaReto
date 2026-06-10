using Microsoft.AspNetCore.Mvc;
using LuminaReto.Models;

namespace LuminaReto.Controllers;

public class ClasificacionController : Controller
{
    private readonly IClasificacionService _clasificacionService;

    public ClasificacionController(IClasificacionService clasificacionService)
    {
        _clasificacionService = clasificacionService;
    }

    public async Task<IActionResult> Index()
    {
        var modelo = new ClasificacionViewModel();

        var idUsuario = HttpContext.Session.GetInt32("IdUsuario");
        if (idUsuario is null)
            return RedirectToAction("Index", "Home");

        try
        {
            var dto = await _clasificacionService.GetRanking(idUsuario.Value);

            if (dto is null)
            {
                modelo.ErrorMessage = "Error al obtener el ranking";
                return View("Clasificacion", modelo);
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
            modelo.ErrorMessage = ex.Message;
        }

        return View("Clasificacion", modelo);
    }
}
