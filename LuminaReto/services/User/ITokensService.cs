public interface ITokensService
{
    Task<List<Recompensa>> GetRecompensas();
    Task<List<Transaccion>> GetTransacciones(int id, string date);
    Task<string> GetUltimaRecompensa(int userId);
    Task<int> GetUserPoints(int id);
    Task UpdatePoints(int id, int points);
    Task CrearTransaccion(int userId, int? recompensaId, int monto, string descripcion);
    Task<int> GetUserPointsMonth(int id);
    Task<string> TokensGraph(int userId);
    Task<List<SkinData>> GetCatalogoSkins();
    Task<List<SkinData>> GetSkinsUsuario(int idUsuario);
    Task ComprarSkin(int idUsuario, int idSkin);
    Task EquiparSkin(int userId, int skinId);
    Task<SkinData?> GetEquippedSkin(int userId); // NEW
}