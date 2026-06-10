using System.Text.Json.Serialization;

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
        [JsonPropertyName("url_foto")]
        public string? UrlFoto     { get; set; }

    }

}