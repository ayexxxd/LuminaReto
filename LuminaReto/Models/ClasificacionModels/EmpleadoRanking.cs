namespace LuminaReto.Models
{
    
    public class EmpleadoRanking
        {
            public int    IdUsuario   { get; set; }
            public int    Posicion    { get; set; }
            public string Nombre      { get; set; }
            public int    WhirlTokens { get; set; }
            public int    TotalPuntos { get; set; }
            public int    RachaActual { get; set; }
            public string? UrlFoto   { get; set; } 


            public string Iniciales =>
                string.IsNullOrWhiteSpace(Nombre) ? "??" :
                string.Join("", Nombre.Split(' ')
                                .Where(p => p.Length > 0)
                                .Take(2)
                                .Select(p => p[0].ToString().ToUpper()));
        }
}