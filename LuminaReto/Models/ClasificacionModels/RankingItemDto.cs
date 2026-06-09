namespace LuminaReto.Models
{
    public class RankingItemDto
    {
        public int    IdUsuario   { get; set; }
        public int    Posicion    { get; set; }
        public string Nombre      { get; set; }
        public int    WhirlTokens { get; set; }
        public int    TotalPuntos { get; set; }
        public int    RachaActual { get; set; }
        public string? UrlFoto   { get; set; } 

    }

}