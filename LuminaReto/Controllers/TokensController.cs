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
        private readonly IUserService _service;

        public TokensController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string DateFilter = "todas", string TypeFilter = "todas")
        {
            //int userId = SessionHelper.GetUserId(HttpContext.Session) ?? 0;
            int userId = 1;
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
                var temppoints = await _service.GetUserPoints(userId);
                var temppointsStr = temppoints.ToString("N0", CultureInfo.InvariantCulture);
                var points = temppointsStr;
                
                foreach (var recompensa in rewards)
                {
                    recompensa.PuedeCanjear = temppoints >= recompensa.Costo;
                    recompensa.TokensFaltantes = recompensa.Costo - temppoints;
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

            //int userId = SessionHelper.GetUserId(HttpContext.Session);
            int userId = 1;
            var points = await _service.GetUserPoints(userId);
            var model = new CanjeConfirmationViewModel
            {
                Recompensa = recompensa,
                TokensActuales = points
            };
            
            return View("~/Views/Home/Tokens.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> Canjear(int recompensaId)
        {
            int userId = 1;
            //int userId = SessionHelper.GetUserId(HttpContext.Session);
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