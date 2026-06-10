using Microsoft.AspNetCore.Mvc;
using LuminaReto.Models.Formularios.ViewModels;
using LuminaReto.Models.Formularios.Dtos;
using LuminaReto.Models.Formularios.Requests;
using LuminaReto.Services.Formularios;

namespace LuminaReto.Controllers;

public class FormulariosController : Controller
{
    private readonly IFormularioService _formularioService;

    public FormulariosController(IFormularioService formularioService)
    {
        _formularioService = formularioService;
    }

    private int? GetIdUsuario() => HttpContext.Session.GetInt32("IdUsuario");
    [HttpGet]
    public IActionResult DebugSesion()
    {
        var id = HttpContext.Session.GetInt32("IdUsuario");
        return Json(new { idUsuario = id });
    }
    public async Task<IActionResult> Index()
    {
        var idUsuario = GetIdUsuario();
        if (idUsuario == null) return RedirectToAction("Login", "Login");

        var disponibles  = await _formularioService.ObtenerFormulariosDisponiblesAsync(idUsuario.Value);
        var completados  = await _formularioService.ObtenerFormulariosCompletadosAsync(idUsuario.Value);
        var progreso     = await _formularioService.ObtenerProgresoAsync(idUsuario.Value);

        var vm = new FormulariosViewModel
        {
            Formularios = disponibles.Select(f => new FormularioVm
            {
                IdFormulario     = f.IdFormulario,
                Titulo           = f.Nombre,
                Descripcion      = "Completa este formulario y gana tokens",
                Tokens           = f.Tokens,
                Preguntas        = f.TotalPreguntas,
                ImagenFormulario = "/imagenes/formularioB.png"
            }).ToList(),

            FormulariosCompletadosLista = completados.Select(f => new FormularioVm
            {
                IdFormulario = f.IdFormulario,
                Titulo       = f.Nombre,
                Tokens       = f.Tokens
            }).ToList(),

            TokensMes              = progreso.TokensMes,
            FormulariosCompletados = progreso.FormulariosCompletados,
            MetaTotal              = progreso.MetaTotal
        };

        return View("Formulario", vm);
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerPreguntas(int idFormulario)
    {
        var preguntas = await _formularioService.ObtenerPreguntasAsync(idFormulario);
        return Json(preguntas);
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerOpciones(int idPregunta)
    {
        var opciones = await _formularioService.ObtenerOpcionesAsync(idPregunta);
        return Json(opciones);
    }

    [HttpPost]
    public async Task<IActionResult> CompletarFormulario([FromBody] CompletarFormularioRequest req)
    {
        var idUsuario = GetIdUsuario();
        if (idUsuario == null) return Unauthorized();

        await _formularioService.CompletarFormularioAsync(idUsuario.Value, req.IdFormulario);
        return Json(new { ok = true });
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerProgreso()
    {
        var idUsuario = GetIdUsuario();
        if (idUsuario == null) return Unauthorized();

        var progreso = await _formularioService.ObtenerProgresoAsync(idUsuario.Value);
        return Json(progreso);
    }
}