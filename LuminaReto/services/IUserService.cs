public interface IUserService
{
   /* Task<User> GetUserById(int id);
    Task UpdatePoints(int id, int points);*/
    Task<List<Recompensa>> GetRecompensas();
    Task<List<Transaccion>> GetTransacciones(int id, string date);
    Task<int> GetUserPoints(int id);
    Task UpdatePoints(int id, int points);
    Task CrearTransaccion(int userId, int recompensaId, int monto, string descripcion);
}
