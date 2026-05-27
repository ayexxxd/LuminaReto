using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LuminaReto.Models;

namespace LuminaReto.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        ModeloInicioGeneral modelo = new ModeloInicioGeneral();

        modelo.ListaEstadisticas = new List<Estadisticas>()
        {
            new Estadisticas { Titulo = "Whirl-Tokens Totales" , Valor = "1,250" , Icono = "imagenes/WTokens.png"},
            new Estadisticas { Titulo = "Formularios Completados" , Valor = "12" , Icono = "imagenes/Formulario.png"},
            new Estadisticas { Titulo = "Nivel Alcanzado" , Valor = "5" , Icono = "imagenes/Nivel.png"},
            new Estadisticas { Titulo = "Racha Activa" , Valor = "7 días" , Icono = "imagenes/Racha.png"}
        };

        modelo.ListaAccionesRapidas = new List<AccionesRapidas>()
        {
            new AccionesRapidas { Texto = "Completar un nuevo formulario" , Controlador = "Home" , Accion = "Formularios" , Icono = "imagenes/Formulario.png"},
            new AccionesRapidas { Texto = "Jugar ahora" , Controlador = "Home" , Accion = "Juego" , Icono = "imagenes/Racha.png"},
            new AccionesRapidas { Texto = "Ver mis Whirl-Tokens" , Controlador = "Home" , Accion = "Tokens" , Icono = "imagenes/WTokens.png"}
        };

        modelo.ListaActividadReciente = new List<ActividadReciente>()
        {
            new ActividadReciente { Descripcion = "Formulario completado" , Tiempo = "Hace 2 horas" , Icono = "imagenes/Formulario.png"},
            new ActividadReciente { Descripcion = "+150 Whirl-Tokens ganados" , Tiempo = "Hace 5 horas" , Icono = "imagenes/WTokens.png"},
            new ActividadReciente { Descripcion = "Nivel alcanzado = 5" , Tiempo = "Hace 1 día" , Icono = "imagenes/Nivel.png"}
        };

        return View(modelo);
    }

    public IActionResult Formularios()
    {
        var formularios = new List<Formulario>
        {
            new Formulario
            {
                Titulo = "Encuesta Laboral",
                Descripcion = "Ayúdanos a mejorar",
                Tokens = 150,
                Preguntas = 8,
                ListaPreguntas = new List<Preguntas>
                {
                    new Preguntas { Texto = "1. ¿Qué tan satisfecho estás con tu ambiente de trabajo?*", Tipo = "escala" },
                    new Preguntas { Texto = "2. ¿Cómo calificarías la comunicación dentro de tu equipo?*", Tipo = "escala" },
                    new Preguntas { Texto = "3. ¿Sientes que tu trabajo es reconocido adecuadamente?*", Tipo = "opcion", Opciones = new List<string> { "Sí", "No", "A veces" } },
                    new Preguntas { Texto = "4. ¿Qué aspecto de tu ambiente laboral mejorarías primero?", Tipo = "abierta" },
                    new Preguntas { Texto = "5. ¿Qué tan cómodo te sientes compartiendo ideas o sugerencias?*", Tipo = "escala" },
                    new Preguntas { Texto = "6. ¿Consideras que tienes las herramientas necesarias para realizar tu trabajo?*", Tipo = "opcion", Opciones = new List<string> { "Sí", "No", "Parcialmente" } },
                    new Preguntas { Texto = "7. ¿Qué tan equilibrada sientes tu carga de trabajo?*", Tipo = "escala" },
                    new Preguntas { Texto = "8. Escribe una sugerencia para mejorar la experiencia laboral.", Tipo = "abierta" }
                }
            },

            new Formulario
            {
                Titulo = "Sugerencias de Mejora Continua",
                Descripcion = "Comparte ideas para mejorar procesos",
                Tokens = 200,
                Preguntas = 12,
                DobleTokens = true,
                ListaPreguntas = new List<Preguntas>
                {
                    new Preguntas { Texto = "1. ¿En qué área o proceso identificaste una oportunidad de mejora?*", Tipo = "abierta" },
                    new Preguntas { Texto = "2. ¿Qué problema observaste en ese proceso?*", Tipo = "abierta" },
                    new Preguntas { Texto = "3. ¿Con qué frecuencia ocurre este problema?*", Tipo = "seleccion", Opciones = new List<string> { "Rara vez", "Algunas veces", "Frecuentemente", "Todos los días" } },
                    new Preguntas { Texto = "4. ¿Qué impacto tiene este problema?*", Tipo = "opcion", Opciones = new List<string> { "Retrasos", "Desperdicio de material", "Errores", "Costos extra", "Mala comunicación", "Otro" } },
                    new Preguntas { Texto = "5. Describe tu propuesta de mejora.*", Tipo = "abierta" },
                    new Preguntas { Texto = "6. ¿Qué recursos se necesitarían para implementar tu idea?*", Tipo = "abierta" },
                    new Preguntas { Texto = "7. ¿Qué beneficio tendría tu propuesta?*", Tipo = "opcion", Opciones = new List<string> { "Mayor eficiencia", "Ahorro de tiempo", "Reducción de errores", "Mejor seguridad", "Mejor comunicación" } },
                    new Preguntas { Texto = "8. ¿Qué tan fácil crees que sería implementar esta mejora?*", Tipo = "escala" },
                    new Preguntas { Texto = "9. ¿Quiénes deberían participar para aplicar esta mejora?*", Tipo = "abierta" },
                    new Preguntas { Texto = "10. ¿Tu propuesta requiere capacitación adicional?*", Tipo = "opcion", Opciones = new List<string> { "Sí", "No", "No estoy seguro" } },
                    new Preguntas { Texto = "11. ¿En cuánto tiempo crees que podría notarse el beneficio?*", Tipo = "seleccion", Opciones = new List<string> { "Inmediato", "1 semana", "1 mes", "Más de 1 mes" } },
                    new Preguntas { Texto = "12. Comentarios adicionales.", Tipo = "abierta" }
                }
            },

            new Formulario
            {
                Titulo = "Evaluación de Seguridad",
                Descripcion = "Reporta condiciones de seguridad",
                Tokens = 100,
                Preguntas = 5,
                Expira = true,
                ListaPreguntas = new List<Preguntas>
                {
                    new Preguntas { Texto = "1. ¿Has identificado algún riesgo de seguridad recientemente?*", Tipo = "opcion", Opciones = new List<string> { "Sí", "No" } },
                    new Preguntas { Texto = "2. ¿En qué área ocurrió o se detectó el riesgo?", Tipo = "abierta" },
                    new Preguntas { Texto = "3. ¿Qué tipo de riesgo identificaste?", Tipo = "seleccion", Opciones = new List<string> { "Equipo dañado", "Piso mojado", "Falta de señalización", "Mala iluminación", "Uso incorrecto de herramientas", "Otro" } },
                    new Preguntas { Texto = "4. ¿Qué nivel de riesgo consideras que tiene?", Tipo = "escala" },
                    new Preguntas { Texto = "5. Describe brevemente la situación o condición insegura.", Tipo = "abierta" }
                }
            }
        };

        return View(formularios);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Juego()
    {
        return View();
    }

    public IActionResult Tokens()
    {
        return View();
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

        return View("Perfil");
    }

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
