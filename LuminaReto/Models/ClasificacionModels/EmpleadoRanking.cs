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
        public bool    EsUsuarioActual { get; set; } 


        public string Iniciales =>
            string.IsNullOrWhiteSpace(Nombre) ? "??" :
            string.Join("", Nombre.Split(' ')
                            .Where(p => p.Length > 0)
                            .Take(2)
                            .Select(p => p[0].ToString().ToUpper()));
        public bool   TieneFoto => !string.IsNullOrEmpty(UrlFoto);  
        public string Medalla   => Posicion switch                   
        {
            1 => "👑", 2 => "🥈", 3 => "🥉", _ => ""
        };
    }
}