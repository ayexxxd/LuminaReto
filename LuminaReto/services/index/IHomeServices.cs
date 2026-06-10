public interface IHomeService
{
    Task<DashboardData> GetDashboard(int idUsuario);
}