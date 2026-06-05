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
		return View("~/Views/Home/Login.cshtml");
	}

	[HttpPost]
	public IActionResult Login(string username, string password, bool remember = false)
	{
		if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
		{
			ModelState.AddModelError(string.Empty, "Ingresa tu correo y contraseña.");
			return View("~/Views/Home/Login.cshtml");
		}

		var userId = _loginService.Login(username, password).Result;

		HttpContext.Session.SetInt32("IdUsuario", userId);
		HttpContext.Session.SetString("LoginRemember", remember ? "true" : "false");

		return RedirectToAction("Index", "Home");
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
