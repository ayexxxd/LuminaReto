using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using LuminaReto.Models;

namespace LuminaReto.Controllers;

[Route("api/[controller]")]
public class LoginController : Controller
{
    private readonly IUserService _userService;

    public LoginController(IUserService userService)
    {
        _userService = userService;
    }

    // GET api/Login/me
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var id = HttpContext.Session.GetInt32("IdUsuario");
        if (id == null)
            return Unauthorized(new { message = "Not authenticated" });

        int userId = id.Value;

        // Populate basic user info using available service methods
        var points = await _userService.GetUserPoints(userId);
        var pointsMonth = await _userService.GetUserPointsMonth(userId);
        var lastReward = await _userService.GetUltimaRecompensa(userId);

        var user = new User
        {
            Id = userId,
            UserNombre = HttpContext.Session.GetString("UserName") ?? "",
            Points = points,
            WhirlTokens = pointsMonth
        };

        return Ok(new { user, lastReward });
    }

    // POST api/Login/authenticate
    // This is a simple placeholder that sets a session value. Replace with real auth as needed.
    [HttpPost("authenticate")]
    [ValidateAntiForgeryToken]
    public IActionResult Authenticate(string username, string password, bool remember = false)
    {
        // TODO: Replace with real authentication logic.
        // For now, sign in a fixed user id and store username in session.
        HttpContext.Session.SetInt32("IdUsuario", 4);
        HttpContext.Session.SetString("UserId", "4");
        HttpContext.Session.SetString("UserName", username ?? "");

        return RedirectToAction("Index", "Home");
    }
}
