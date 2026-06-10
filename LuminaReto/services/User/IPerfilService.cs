using System.Threading.Tasks;
using LuminaReto.Models; 

namespace LuminaReto.Services 
{
    public interface IPerfilService
    {
        Task<PerfilUsuario> GetPerfil(int idUsuario);
        Task<bool> EditarPerfil(int idUsuario, string nombre, string correo);
        Task<EstadisticasUsuario> GetEstadisticas(int idUsuario);
        Task<bool> GuardarPerfil(PerfilUsuario perfil);
    }
}