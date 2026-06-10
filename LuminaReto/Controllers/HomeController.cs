using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LuminaReto.Models;
using Microsoft.AspNetCore.Http;
using Perfil.Models;
using System.Text.Json;

namespace LuminaReto.Controllers;

public class HomeController : Controller
{
    /*ACCIÓN INDEX*/
    private readonly IHomeService _service;

public HomeController(IHomeService service)
{
    _service = service;
}

public async Task<IActionResult> Index()
{
    var userId = HttpContext.Session.GetInt32("IdUsuario") ?? 0;

    var dashboard = await _service.GetDashboard(userId);

    var modelo = new ModeloInicioGeneral
    {
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
<<<<<<< HEAD
            new() { Descripcion = "Formulario completado", Tiempo = "Hace 2 horas", Icono = "/imagenes/Formulario.png" },
            new() { Descripcion = "+150 Whirl-Tokens ganados", Tiempo = "Hace 5 horas", Icono = "/imagenes/WTokens.png" },
            new() { Descripcion = "Nivel alcanzado = 5", Tiempo = "Hace 1 día", Icono = "/imagenes/Nivel.png" }
        }
    };

    return View(modelo);
}
=======
            new ActividadReciente { Descripcion = "Formulario completado" , Tiempo = "Hace 2 horas" , Icono = "/imagenes/Formulario.png"},
            new ActividadReciente { Descripcion = "+150 Whirl-Tokens ganados" , Tiempo = "Hace 5 horas" , Icono = "/imagenes/WTokens.png"},
            new ActividadReciente { Descripcion = "Nivel alcanzado = 5" , Tiempo = "Hace 1 día" , Icono = "/imagenes/Nivel.png"}
        };
        var userId  = HttpContext.Session.GetInt32("IdUsuario") ?? 1;
        var baseUrl = "https://datastudio.google.com/embed/reporting/365a6ae2-42a4-40f5-8641-88b6aad2f7ca/page/9Pn0F";
        var parameters = new { 
            Bid_usuario  = userId,
            Tid_usuario  = userId,
            Eid_usuario  = userId,
            Rid_usuario  = userId,
            T5id_usuario = userId,
            T6d_usuario  = userId
        };
        var json    = JsonSerializer.Serialize(parameters);
        var encoded = Uri.EscapeDataString(json);
        modelo.DashboardUrl = baseUrl + "?params=" + encoded;
        return View(modelo);
    }
>>>>>>> 3eca4365709a965adb44965fe71d916f7861c40d

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

    public IActionResult Perfil()
    {
        ViewData["NombreCliente"] = "Juan Pérez";
        ViewData["DepaCliente"] = "Marketing";
        ViewData["CorreoCliente"] = "juanperez@whirlpool.com";
        ViewData["FechaRegistro"] = new DateTime(2024, 3, 15).ToString("dd/MM/yyyy");
        Estadisticas();
        Departamentos();

        return View();
    }

    private void Estadisticas()
    {
        ViewData["Tokens"] = 1500;
        ViewData["Racha"] = 20;
        ViewData["Formularios"] = 110;
    }

    private void Departamentos()
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

    private void Confirmacion(string modo)
    {
        if (modo != "editar")
        {
            TempData["Confirmacion"] = new Confirmacion
            {
                Mensaje = "Cambios guardados correctamente",
                Tipo = "success"
            };
        }
    }

    [HttpPost]
    public IActionResult Perfil(string nombre, string correo, string departamento, string modo) 
    {
        ViewData["NombreCliente"] = nombre;
        ViewData["CorreoCliente"] = correo;
        ViewData["DepaCliente"] = departamento;
        ViewData["FechaRegistro"] = new DateTime(2024, 3, 15).ToString("dd/MM/yyyy"); 
        Estadisticas();
        Departamentos();

        if (modo == "editar")
        {
            ViewData["Editable"] = true;
        }
        else
        {
            ViewData["Editable"] = false;
            Confirmacion(modo);
        }

        return View("Perfil");
    }
    
}
