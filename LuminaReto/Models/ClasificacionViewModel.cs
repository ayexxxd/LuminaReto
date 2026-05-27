namespace LuminaReto.Models
{
    using System.Text.Json.Serialization;

    public class EmpleadoRanking
    {
        public int    IdUsuario   { get; set; }
        public int    Posicion    { get; set; }
        public string Nombre      { get; set; }
        public int    WhirlTokens { get; set; }
        public int    TotalPuntos { get; set; }
        public int    RachaActual { get; set; }

        public string Iniciales =>
            string.IsNullOrWhiteSpace(Nombre) ? "??" :
            string.Join("", Nombre.Split(' ')
                            .Where(p => p.Length > 0)
                            .Take(2)
                            .Select(p => p[0].ToString().ToUpper()));
    }

    public class ClasificacionViewModel
    {
        public List<EmpleadoRanking> Top3          { get; set; } = new();
        public List<EmpleadoRanking> Ranking       { get; set; } = new();
        public EmpleadoRanking       UsuarioActual { get; set; }
        public string                ErrorMessage  { get; set; }
    }

    public class RankingItemDto
    {
        public int    IdUsuario   { get; set; }
        public int    Posicion    { get; set; }
        public string Nombre      { get; set; }
        public int    WhirlTokens { get; set; }
        public int    TotalPuntos { get; set; }
        public int    RachaActual { get; set; }
    }

    public class RankingResponseDto
    {
        [JsonPropertyName("ranking")]
        public List<RankingItemDto> Ranking { get; set; } = new();

        [JsonPropertyName("usuario_actual")]
        public RankingItemDto? UsuarioActual { get; set; }
    }
}