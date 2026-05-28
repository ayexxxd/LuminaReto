using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LuminaReto.Helpers;
using LuminaReto.Models;

namespace LuminaReto.Controllers
{
    public class TokensController : Controller
    {
        private readonly IUserService _service;

        public TokensController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string DateFilter = "todas", string TypeFilter = "todas")
        {
            int userId = SessionHelper.GetUserId(HttpContext.Session);
            try
            {
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
                
                var ganados = await _service.GetUserPointsMonth(userId);
                var rewards = await _service.GetRecompensas();
                string ultimaRecompensa = await _service.GetUltimaRecompensa(userId);
                var points = await _service.GetUserPoints(userId);

                foreach (var recompensa in rewards)
                {
                    recompensa.PuedeCanjear = points >= recompensa.Costo;
                    recompensa.TokensFaltantes = recompensa.Costo - points;
                }

                //viewModel
                var model = new TokenViewModel
                {
                    WhirlTokens = points,
                    GanadosMes = ganados,
                    UltimaRecompensa = ultimaRecompensa,

                    DateFilter = DateFilter,
                    TypeFilter = TypeFilter,

                    ListaTransacciones = transactions,
                    ListaRecompensas = rewards
                };
                return View("~/Views/Home/Tokens.cshtml", model);
            }
            catch(Exception)
            {
                TempData["SuccessMessage"] = "Error al Cargar Datos";

                return View("~/Views/Home/Tokens.cshtml", new TokenViewModel{
                ListaTransacciones = new List<Transaccion>(),
                ListaRecompensas = new List<Recompensa>()}
                );
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmarCanje(int recompensaId)
        {
            var rewards = await _service.GetRecompensas();
            var recompensa = rewards.FirstOrDefault(r => r.IdRecompensa == recompensaId);

            if (recompensa == null)
            {
                TempData["SuccessMessage"] = "La recompensa no existe";
                return Redirect("/Tokens/Index");
            }

            int userId = SessionHelper.GetUserId(HttpContext.Session);
            var points = await _service.GetUserPoints(userId);
            var model = new CanjeConfirmationViewModel
            {
                Recompensa = recompensa,
                TokensActuales = points
            };
            
            return View("~/Views/Tokens/ConfirmarCanje.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> Canjear(int recompensaId)
        {
            int userId = SessionHelper.GetUserId(HttpContext.Session);
            var rewards = await _service.GetRecompensas();
            var recompensa = rewards.FirstOrDefault(r => r.IdRecompensa == recompensaId);
            
            if (recompensa == null)
            {
                TempData["SuccessMessage"] = "La recompensa no existe";
                return Redirect("/Tokens/Index");
            }
        
            var points = await _service.GetUserPoints(userId);
            if (points < recompensa.Costo)
            {
                return Redirect("/Tokens/Index");
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