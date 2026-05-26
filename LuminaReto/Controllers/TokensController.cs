using System;
using System.Linq;
using System.Threading.Tasks;
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
            string fecha = "2000-01-01";

            switch (DateFilter)
            {
                case "hoy":
                    fecha = DateTime.Today.ToString("yyyy-MM-dd");
                    break;

                case "semana":
                    fecha = DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd");
                    break;

                case "mes":
                    fecha = DateTime.Today.AddMonths(-1).ToString("yyyy-MM-dd");
                    break;
            }

            var transactions = await _service.GetTransacciones(1, fecha);
            var rewards = await _service.GetRecompensas();
            var points = await _service.GetUserPoints(1);

            int tokensActuales = points;

            // Tokens ganados
            int ganados = transactions.Where(t => t.Monto > 0).Sum(t => t.Monto);

            // Próxima recompensa
            var proxima = rewards.Where(r => r.Costo > tokensActuales).OrderBy(r => r.Costo).FirstOrDefault();

            foreach (var recompensa in rewards)
            {
                recompensa.PuedeCanjear = points >= recompensa.Costo;
                recompensa.TokensFaltantes = recompensa.Costo - points;
            }

            int costoProxima = proxima?.Costo ?? tokensActuales;

            // Progreso
            int target = Math.Max(costoProxima, tokensActuales);

            int porcentaje = target > 0 
                ? (int)((double)tokensActuales / target * 100) : 0;

            int faltantes = target - tokensActuales;

            // ViewModel
            var model = new TokenViewModel
            {
                WhirlTokens = points,
                GanadosMes = ganados,
                ProximaRecompensa = costoProxima,
                TargetProgreso = target,
                PorcentajeProgreso = porcentaje,
                RestantesProgreso = faltantes,
                ListaTransacciones = transactions,
                ListaRecompensas = rewards
            };

            return View("~/Views/Home/Tokens.cshtml", model);
        }

        public IActionResult Regresar()
        {
            return RedirectToAction(nameof(Index), "Home");
        }

        [HttpPost]
    public async Task<IActionResult> Canjear(int recompensaId)
    {
        int userId = 1;

        var rewards = await _service.GetRecompensas();

        var recompensa = rewards.FirstOrDefault(r => r.Id == recompensaId);

        if (recompensa == null)
        {
            return RedirectToAction(nameof(Index));
        }
        var points = await _service.GetUserPoints(1);

        if (points < recompensa.Costo)
        {
            return RedirectToAction(nameof(Index));
        }

        await _service.UpdatePoints(userId, -recompensa.Costo);

        await _service.CrearTransaccion(userId, recompensaId, -recompensa.Costo,"Canjeó: " + recompensa.NombreRecompensa
    );

    return RedirectToAction(nameof(Index));
}
    }}