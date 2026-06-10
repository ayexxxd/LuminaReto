using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LuminaReto.Models;
using Microsoft.AspNetCore.Http;

namespace LuminaReto.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        ModeloInicioGeneral modelo = new ModeloInicioGeneral();

        modelo.ListaEstadisticas = new List<Estadisticas>()
        {
            new Estadisticas { Titulo = "Whirl-Tokens Totales"     , Valor = "1,250"  , Icono = "/imagenes/WTokens.png"   },
            new Estadisticas { Titulo = "Formularios Completados"  , Valor = "12"     , Icono = "/imagenes/Formulario.png" },
            new Estadisticas { Titulo = "Nivel Alcanzado"          , Valor = "5"      , Icono = "/imagenes/Nivel.png"      },
            new Estadisticas { Titulo = "Racha Activa"             , Valor = "7 días" , Icono = "/imagenes/Racha.png"      }
        };

        modelo.ListaAccionesRapidas = new List<AccionesRapidas>()
        {
            new AccionesRapidas { Texto = "Completar un nuevo formulario", Controlador = "Home"  , Accion = "Formularios", Icono = "/imagenes/Formulario.png" },
            new AccionesRapidas { Texto = "Jugar ahora"                  , Controlador = "Home"  , Accion = "Juego"      , Icono = "/imagenes/Racha.png"      },
            new AccionesRapidas { Texto = "Ver mis Whirl-Tokens"         , Controlador = "Tokens", Accion = "Index"      , Icono = "/imagenes/WTokens.png"    }
        };

        modelo.ListaActividadReciente = new List<ActividadReciente>()
        {
            DashboardUrl = baseUrl + "?params=" + encoded,
            ListaEstadisticas = new List<Estadisticas>
            {
                new() { Titulo = "Whirl-Tokens Totales", Valor = dashboard.WhirlTokens.ToString("N0"), Icono = "/imagenes/WTokens.png" },
                new() { Titulo = "Formularios Completados", Valor = dashboard.FormulariosTotales.ToString(), Icono = "/imagenes/Formulario.png" },
                new() { Titulo = "Puntos", Valor = dashboard.Puntos.ToString("N0"), Icono = "/imagenes/Nivel.png" },
                new() { Titulo = "Racha Activa", Valor = dashboard.RachaActual + " días", Icono = "/imagenes/Racha.png" }
            },
            ListaAccionesRapidas = new List<AccionesRapidas>
            {
                new() { Texto = "Completar un nuevo formulario", Controlador = "Home", Accion = "Formularios", Icono = "/imagenes/Formulario.png" },
                new() { Texto = "Jugar ahora", Controlador = "Home", Accion = "Juego", Icono = "/imagenes/Racha.png" },
                new() { Texto = "Ver mis Whirl-Tokens", Controlador = "Tokens", Accion = "Index", Icono = "/imagenes/WTokens.png" }
            },
            ListaActividadReciente = new List<ActividadReciente>
            {
                new() { Descripcion = "Formulario completado", Icono = "/imagenes/Formulario.png" },
                new() { Descripcion = "+150 Whirl-Tokens ganados", Icono = "/imagenes/WTokens.png" },
                new() { Descripcion = "Puntos = 5", Icono = "/imagenes/Nivel.png" }
            }
        };

        return View(modelo);
    }

    public IActionResult Juego()
    {
        return View();
    }
    public async Task<IActionResult> Tokens()
    {
        return RedirectToAction("Index", "Tokens");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost]
    public IActionResult Login()
    {
        HttpContext.Session.SetInt32("IdUsuario", 4);
        return RedirectToAction("Index", "Home");
    }
}