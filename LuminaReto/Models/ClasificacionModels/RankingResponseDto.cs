namespace LuminaReto.Models
{
    using System.Text.Json.Serialization;

    public class RankingResponseDto
    {
        [JsonPropertyName("ranking")]
        public List<RankingItemDto> Ranking { get; set; } = new();

        [JsonPropertyName("usuario_actual")]
        public RankingItemDto? UsuarioActual { get; set; }
    }
}