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
                var userId = HttpContext.Session.GetInt32("IdUsuario") ?? 0;             //OBTENER ID USUARIO DE LA SESIÓN
                if (userId == 0)
                    return RedirectToAction("Login", "Login");                           //REDIRIGIR A LOGIN SI NO HAY USUARIO EN SESIÓN

                string fechaStandard = "2000-01-01";                        
                switch (DateFilter)                                                      //FILTRO DE FECHA PARA TRANSACCIONES
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
                var transactions = await _service.GetTransacciones(userId, fechaStandard); //TRANSACCIONES DEL USUARIO

                if (TypeFilter == "ganadas")
                    transactions = transactions.Where(t => t.Monto > 0).ToList();
                else if (TypeFilter == "gastadas")
                    transactions = transactions.Where(t => t.Monto < 0).ToList();          //FILTRO DE TIPO DE TRANSACCIONES

                var temppoints = await _service.GetUserPoints(userId);                  //PUNTOS TOTALES DEL USUARIO
                var points = temppoints.ToString("N0", CultureInfo.InvariantCulture);   //PUNTOS TOTALES DEL USUARIO FORMATEADOS

                var tempganados = await _service.GetUserPointsMonth(userId);            //PUNTOS GANADOS EN EL MES
                var ganados = tempganados.ToString("N0", CultureInfo.InvariantCulture); //PUNTOS GANADOS EN EL MES FORMATEADOS

                var rewards = await _service.GetRecompensas();                          //RECOMPENSAS DISPONIBLES
                string ultimaRecompensa = await _service.GetUltimaRecompensa(userId);   //ULTIMA RECOMPENSA CANJEADA

                foreach (var recompensa in rewards)                                     //VERIFICAR SI EL USUARIO PUEDE CANJEAR CADA RECOMPENSA
                {
                    recompensa.PuedeCanjear = temppoints >= recompensa.Costo;
                    recompensa.TokensFaltantes = recompensa.Costo - temppoints;
                }

                //SKIN CATALOG
                var catalogo = await _service.GetCatalogoSkins();
                var skinsUsuario = await _service.GetSkinsUsuario(userId);
                var skinIds = skinsUsuario.Select(s => s.IdSkin).ToHashSet();

                foreach (var skin in catalogo)
                {
                    skin.Owned = skinIds.Contains(skin.IdSkin);
                    skin.Equipada = skinsUsuario.FirstOrDefault(s => s.IdSkin == skin.IdSkin)?.Equipada ?? false; // NEW
                    skin.PuedeComprar = !skin.Owned && temppoints >= skin.Costo;
                    skin.TokensFaltantes = skin.Costo - temppoints;
                }
                //SKIN CATALOG END

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
                    ListaSkins = catalogo,
                    SkinsUsuario = skinsUsuario,
                    TokensGraphUrl = await _service.TokensGraph(userId)
                };

                return View("~/Views/Home/Tokens.cshtml", model);
            }
            catch (Exception x)
            {
                TempData["Message"] = "Error al Cargar Datos: ";
                return View("~/Views/Home/Tokens.cshtml", new TokenViewModel
                {
                    ListaTransacciones = new List<Transaccion>(),
                    ListaRecompensas = new List<Recompensa>(),
                    ListaSkins = new List<SkinData>(),
                    SkinsUsuario = new List<SkinData>()
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Canjear(int recompensaId)
        {
            var userId = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            if (userId == 0)
                return RedirectToAction("Login", "Login");

            var rewards = await _service.GetRecompensas();
            var recompensa = rewards.FirstOrDefault(r => r.IdRecompensa == recompensaId);

            var points = await _service.GetUserPoints(userId);
            if (points < recompensa.Costo)
                return Redirect("/Tokens");

            await _service.UpdatePoints(userId, -recompensa.Costo);
            await _service.CrearTransaccion(userId, recompensaId, -recompensa.Costo, "Canjeó: " + recompensa.NombreRecompensa);
            TempData["Message"] = "Canjeaste " + recompensa.NombreRecompensa;
            return Redirect("/Tokens/Index");
        }

        [HttpPost]
        public async Task<IActionResult> ComprarSkin(int skinId)
        {
            var userId = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            if (userId == 0)
                return RedirectToAction("Login", "Login");

            var catalogo = await _service.GetCatalogoSkins();
            var skin = catalogo.FirstOrDefault(s => s.IdSkin == skinId);

            var points = await _service.GetUserPoints(userId);
            if (points < skin.Costo)
            {
                TempData["Message"] = "No tienes suficientes tokens";
                return Redirect("/Tokens");
            }

            // check ownership first
            var skinsUsuario = await _service.GetSkinsUsuario(userId);
            if (skinsUsuario.Any(s => s.IdSkin == skinId))
            {
                TempData["Message"] = "Ya tienes esta skin";
                return Redirect("/Tokens/Index");
            }

    await _service.ComprarSkin(userId, skinId);
    await _service.UpdatePoints(userId, -skin.Costo);
    await _service.CrearTransaccion(userId, null, -skin.Costo, "Compró música: " + skin.Nombre);
    TempData["Message"] = "¡Compraste " + skin.Nombre + "!";
    return Redirect("/Tokens/Index");
}
        [HttpPost]
        public async Task<IActionResult> EquiparSkin(int skinId)
        {
            var userId = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            if (userId == 0)
                return RedirectToAction("Login", "Login");

            await _service.EquiparSkin(userId, skinId);
            TempData["Message"] = "Skin equipada";
            return Redirect("/Tokens/Index");
        }
        
        public IActionResult Regresar()
        {
            return RedirectToAction(nameof(Index), "Home");
        }
}}