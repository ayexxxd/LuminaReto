using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LuminaReto.Models;
using Microsoft.AspNetCore.Http;

namespace LuminaReto.Controllers;

public class HomeController : Controller
{
    /*ACCIÓN INDEX*/
    public IActionResult Index()
    {
        ModeloInicioGeneral modelo = new ModeloInicioGeneral();

        modelo.ListaEstadisticas = new List<Estadisticas>()
        {
            new Estadisticas { Titulo = "Whirl-Tokens Totales" , Valor = "1,250" , Icono = "/imagenes/WTokens.png"},
            new Estadisticas { Titulo = "Formularios Completados" , Valor = "12" , Icono = "/imagenes/Formulario.png"},
            new Estadisticas { Titulo = "Nivel Alcanzado" , Valor = "5" , Icono = "/imagenes/Nivel.png"},
            new Estadisticas { Titulo = "Racha Activa" , Valor = "7 días" , Icono = "/imagenes/Racha.png"}
        };

        modelo.ListaAccionesRapidas = new List<AccionesRapidas>()
        {
            new AccionesRapidas { Texto = "Completar un nuevo formulario" , Controlador = "Home" , Accion = "Formularios" , Icono = "/imagenes/Formulario.png"},
            new AccionesRapidas { Texto = "Jugar ahora" , Controlador = "Home" , Accion = "Juego" , Icono = "/imagenes/Racha.png"},
            new AccionesRapidas { Texto = "Ver mis Whirl-Tokens" , Controlador = "Tokens" , Accion = "Index" , Icono = "/imagenes/WTokens.png"}
        };

        modelo.ListaActividadReciente = new List<ActividadReciente>()
        {
            new ActividadReciente { Descripcion = "Formulario completado" , Tiempo = "Hace 2 horas" , Icono = "/imagenes/Formulario.png"},
            new ActividadReciente { Descripcion = "+150 Whirl-Tokens ganados" , Tiempo = "Hace 5 horas" , Icono = "/imagenes/WTokens.png"},
            new ActividadReciente { Descripcion = "Nivel alcanzado = 5" , Tiempo = "Hace 1 día" , Icono = "/imagenes/Nivel.png"}
        };

        return View(modelo);
    }

    /*ACCIÓN JUEGO*/
    public IActionResult Juego()
    {
        return View();
    }

    /*ACCIÓN TOKENS — redirige al controlador de tokens que maneja la API*/
    public async Task<IActionResult> Tokens()
    {
        return RedirectToAction("Index", "Tokens");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    /*ACCIÓN ERROR*/
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    /*ACCIÓN LOGIN — guarda el id de usuario en sesión*/
    [HttpPost]
    public IActionResult Login()
    {
        HttpContext.Session.SetInt32("IdUsuario", 4);
        return RedirectToAction("Index", "Home");
    }

    /*ACCIÓN PERFIL POST — recibe datos del formulario y los guarda*/
    [HttpPost]
    public IActionResult Perfil(string nombre, string correo, string departamento, string id, string modo)
    {
        ViewData["NombreCliente"] = nombre;
        ViewData["CorreoCliente"] = correo;
        ViewData["DepaCliente"] = departamento;
        ViewData["IdCliente"] = id;

        if (modo == "editar")
        {
            ViewData["Editable"] = true;
        }
        else
        {
            ViewData["Editable"] = false;
        }

        // Guarda el id en sesión para que otras páginas (Tokens) puedan leerlo
        if (!string.IsNullOrEmpty(id))
        {
            HttpContext.Session.SetString("UserId", id);
        }

        return View("Perfil");
    }

    /*ACCIÓN PERFIL GET — muestra la página la primera vez*/
    public IActionResult Perfil()
    {
        ViewData["NombreCliente"] = "";
        ViewData["DepaCliente"] = "";
        ViewData["CorreoCliente"] = "";
        ViewData["IdCliente"] = "";
        ViewData["Editable"] = false;
        return View();
    }
}
