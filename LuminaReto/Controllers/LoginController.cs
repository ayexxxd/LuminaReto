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
		if (string.IsNullOrWhiteSpace(username) && string.IsNullOrWhiteSpace(password))
		{
			TempData["Message"] = "Ingresa tu correo y contraseña.";
			ViewData["Username"] = username;
			return View("~/Views/Home/Login.cshtml");
		}

		if (string.IsNullOrWhiteSpace(username))
		{
			TempData["Message"] = "El correo electrónico es obligatorio.";
			ViewData["Username"] = username;
			return View("~/Views/Home/Login.cshtml");
		}

		if (!username.Contains("@") || !username.Contains("."))
		{
			TempData["Message"] = "El correo electrónico no es válido.";
			ViewData["Username"] = username;
			return View("~/Views/Home/Login.cshtml");
		}

		if (string.IsNullOrWhiteSpace(password))
		{
			TempData["Message"] = "La contraseña es obligatoria.";
			ViewData["Username"] = username;
			return View("~/Views/Home/Login.cshtml");
		}

		try
		{
			var email = username.ToLower();
			var userId = await _loginService.Login(email, password);

			if (userId <= 0)
			{
				TempData["Message"] = "Usuario o contraseña incorrectos.";
				ViewData["Username"] = username;
				return View("~/Views/Home/Login.cshtml");
			}

			HttpContext.Session.SetInt32("IdUsuario", userId);
			return RedirectToAction("Index", "Home");
		}
		catch (HttpRequestException)
		{
			TempData["Message"] = "No se pudo conectar con el servidor de autenticación.";
			ViewData["Username"] = username;
			return View("~/Views/Home/Login.cshtml");
		}
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public IActionResult Logout()
	{
		HttpContext.Session.Remove("IdUsuario");
		return RedirectToAction("Index", "Home");
	}
}
