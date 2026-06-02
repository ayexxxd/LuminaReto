using Microsoft.AspNetCore.Mvc;

namespace LuminaReto.Controllers;

public class LoginController : Controller
{
	[HttpGet]
	public IActionResult Login()
	{
		return View("~/Views/Home/Login.cshtml");
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public IActionResult Login(string username, string password, bool remember = false)
	{
		if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
		{
			ModelState.AddModelError(string.Empty, "Ingresa tu correo y contraseña.");
			return View("~/Views/Home/Login.cshtml");
		}

		HttpContext.Session.SetInt32("IdUsuario", 4);
		HttpContext.Session.SetString("UserEmail", username.Trim());
		HttpContext.Session.SetString("LoginRemember", remember ? "true" : "false");

		return RedirectToAction("Index", "Home");
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public IActionResult Logout()
	{
		HttpContext.Session.Remove("IdUsuario");
		HttpContext.Session.Remove("UserEmail");
		HttpContext.Session.Remove("LoginRemember");

		return RedirectToAction("Index", "Home");
	}
}
