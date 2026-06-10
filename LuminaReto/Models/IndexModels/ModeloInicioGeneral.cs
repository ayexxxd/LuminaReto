
namespace LuminaReto.Models
{
    public class ModeloInicioGeneral 
    {
        public List<Estadisticas> ListaEstadisticas {get; set;} 
        public List<AccionesRapidas> ListaAccionesRapidas {get; set;} 
        public List<ActividadReciente> ListaActividadReciente {get; set;} 
        public string DashboardUrl { get; set; }
    }
    
}