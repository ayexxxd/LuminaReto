using Microsoft.AspNetCore.Mvc;

namespace LuminaReto.Controllers;

public class LoginController : Controller
{
	private readonly ILoginService _loginService;
	public LoginController(ILoginService loginService)
	{
		_loginService = loginService;
	}

	[HttpGet]
	public IActionResult Login()
	{
		if (HttpContext.Session.GetInt32("IdUsuario") != null)
		{
			return RedirectToAction("Index", "Home");
		}
		return View("~/Views/Home/Login.cshtml");
	}

	[HttpPost]
	public async Task<IActionResult> Login(string username, string password)
	{
		if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
		{
			ModelState.AddModelError(string.Empty, "Ingresa tu correo y contraseña.");
			return View("~/Views/Home/Login.cshtml");
		}

		try
		{
			var userId = await _loginService.Login(username, password);

			if (userId <= 0)
			{
				ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
				return View("~/Views/Home/Login.cshtml");
			}

			HttpContext.Session.SetInt32("IdUsuario", userId);
			return RedirectToAction("Index", "Home");
		}
		catch (HttpRequestException)
		{
			ModelState.AddModelError(string.Empty, "No se pudo conectar con el servidor de autenticación. Inténtalo más tarde.");
			return View("~/Views/Home/Login.cshtml");
		}
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public IActionResult Logout()
	{
		HttpContext.Session.Remove("IdUsuario");
		HttpContext.Session.Remove("LoginRemember");
		return RedirectToAction("Index", "Home");
	}
}
