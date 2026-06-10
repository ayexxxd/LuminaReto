using Microsoft.AspNetCore.Mvc;
using LuminaReto.Models;
using LuminaReto.Services;
using Perfil.Models;

namespace LuminaReto.Controllers;

public class PerfilController : Controller
{
    private readonly IPerfilService _perfilService;

    public PerfilController(IPerfilService perfilService)
    {
        _perfilService = perfilService;
    }

    public async Task<IActionResult> Perfil()
    {
        int idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;

        if (idUsuario == 0)
            return RedirectToAction("Index", "Home");

        var perfil = await _perfilService.GetPerfil(idUsuario);
        var stats  = await _perfilService.GetEstadisticas(idUsuario);

        CargarViewData(perfil, stats);
        CargarDepartamentos();

        return View("~/Views/Home/Perfil.cshtml");
    }

    [HttpPost]
    public async Task<IActionResult> Perfil(string nombre, string correo,
                                        string departamento, string modo)
    {
        int idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;

        if (idUsuario == 0)
            return RedirectToAction("Index", "Home");

        if (modo != "editar")
        {
            bool ok = await _perfilService.EditarPerfil(idUsuario, nombre, correo);

            TempData["Confirmacion"] = new Confirmacion
            {
                Mensaje = ok ? "Cambios guardados correctamente"
                            : "No se pudieron guardar los cambios. Intenta de nuevo.",
                Tipo    = ok ? "success" : "error"
            };

            ViewData["Editable"] = false;
        }
        else
        {
            ViewData["Editable"] = true;
        }

        var perfil = await _perfilService.GetPerfil(idUsuario);
        var stats  = await _perfilService.GetEstadisticas(idUsuario);

        if (perfil != null)
        {
            CargarViewData(perfil, stats);
        }
        else
        {
            ViewData["NombreCliente"] = nombre;
            ViewData["CorreoCliente"] = correo;
            ViewData["DepaCliente"]   = departamento;
            ViewData["FechaRegistro"] = "";
            ViewData["Tokens"]        = stats?.TokensGanados ?? 0;
            ViewData["Racha"]         = stats?.RachaMaxima   ?? 0;
            ViewData["Formularios"]   = stats?.FormulariosTotales ?? 0;
        }

        CargarDepartamentos();

        return View("~/Views/Home/Perfil.cshtml");
    }

    private void CargarViewData(PerfilUsuario perfil, EstadisticasUsuario stats)
    {
        ViewData["NombreCliente"] = perfil?.Nombre       ?? "";
        ViewData["DepaCliente"]   = perfil?.Departamento ?? "";
        ViewData["CorreoCliente"] = perfil?.Correo       ?? "";
        ViewData["FechaRegistro"] = perfil?.Fecha_registro ?? "";

        ViewData["Tokens"]      = stats?.TokensGanados    ?? 0;
        ViewData["Racha"]       = stats?.RachaMaxima      ?? 0;
        ViewData["Formularios"] = stats?.FormulariosTotales ?? 0;

        ViewData["FotoUrl"] = perfil?.url_foto ?? "";
    }

    private void CargarDepartamentos()
    {
        ViewData["ListaDepartamentos"] = new List<string>
        {
            "Marketing",
            "Recursos Humanos",
            "IT",
            "Producción",
            "Logística",
            "Ventas",
            "Legal y Cumplimiento",
            "Finanzas y Administración",
            "Atención al Cliente",
            "Cadena de Suministro",
            "Manufactura y Operaciones",
            "Ingeniería y Tecnología",
            "Gestión de Proyectos"
        };
    }
}