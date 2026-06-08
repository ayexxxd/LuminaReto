using System;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using LuminaReto.Models;

namespace LuminaReto.Controllers
{
    public class TokensController : Controller
    {
        private readonly ITokensService _service;

        public TokensController(ITokensService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string DateFilter = "todas", string TypeFilter = "todas")
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
                if (userId == 0)
                {
                    return RedirectToAction("Login", "Login");
                }

                string fechaStandard = "2000-01-01";
                switch (DateFilter)//DATE FILTER
                {
                    case "hoy":
                        fechaStandard = DateTime.Today.ToString("yyyy-MM-dd");
                        break;

                    case "semana":
                        fechaStandard = DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd");
                        break;

                    case "mes":
                        fechaStandard = DateTime.Today.AddMonths(-1).ToString("yyyy-MM-dd");
                        break;
                }
                var transactions = await _service.GetTransacciones(userId, fechaStandard);
                
                if (TypeFilter == "ganadas")//TYPE FILTER//
                {
                    transactions = transactions.Where(t => t.Monto > 0).ToList();
                }
                else if (TypeFilter == "gastadas")
                {
                    transactions = transactions.Where(t => t.Monto < 0).ToList();
                }
                
                var tempganados = await _service.GetUserPointsMonth(userId);
                var ganados = tempganados.ToString("N0", CultureInfo.InvariantCulture);
                var rewards = await _service.GetRecompensas();
                string ultimaRecompensa = await _service.GetUltimaRecompensa(userId);
                var temppoints = await _service.GetUserPoints(userId);
                var points = temppoints.ToString("N0", CultureInfo.InvariantCulture);
                
                foreach (var recompensa in rewards)
                {
                    recompensa.PuedeCanjear = temppoints >= recompensa.Costo;
                    recompensa.TokensFaltantes = recompensa.Costo - temppoints;
                }
                //viewModel
                var model = new TokenViewModel
                {
                    IdUser = userId,
                    WhirlTokens = points,
                    GanadosMes = ganados,
                    UltimaRecompensa = ultimaRecompensa,

                    DateFilter = DateFilter,
                    TypeFilter = TypeFilter,

                    ListaTransacciones = transactions,
                    ListaRecompensas = rewards,
                    TokensGraphUrl = await _service.TokensGraph(userId)
                };
                return View("~/Views/Home/Tokens.cshtml", model);
            }
            catch (Exception ex)
            {
                TempData["SuccessMessage"] = "Error al Cargar Datos: " + ex.Message;

                return View("~/Views/Home/Tokens.cshtml", new TokenViewModel{
                ListaTransacciones = new List<Transaccion>(),
                ListaRecompensas = new List<Recompensa>()}
                );
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Canjear(int recompensaId)
        {
            var userId = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            if (userId == 0)
            {
                return RedirectToAction("Login", "Login");
            }

            var rewards = await _service.GetRecompensas();
            var recompensa = rewards.FirstOrDefault(r => r.IdRecompensa == recompensaId);
        
            var points = await _service.GetUserPoints(userId);
            if (points < recompensa.Costo)
            {
                return Redirect("/Tokens");
            }

            await _service.UpdatePoints(userId, -recompensa.Costo);
            await _service.CrearTransaccion(userId, recompensaId, -recompensa.Costo,"Canjeó: " + recompensa.NombreRecompensa);
            TempData["SuccessMessage"] = "Canjeaste " + recompensa.NombreRecompensa;
            return Redirect("/Tokens/Index");
        }
    public IActionResult Regresar()
        {
            return RedirectToAction(nameof(Index), "Home");
        }
    }
}