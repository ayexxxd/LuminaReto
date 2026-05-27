using LuminaReto.Models;

public interface IClasificacionService
{
    Task<RankingResponseDto?> GetRanking(int idUsuario, int limite = 20);
}
