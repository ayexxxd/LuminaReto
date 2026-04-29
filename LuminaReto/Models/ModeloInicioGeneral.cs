/* Aqui va el modelo general de la página de inicio, junta toda la información que se mostrará en la pantalla */

namespace LuminaReto.Models
{
    public class ModeloInicioGeneral /*Crea el modelo principal que se mandará desde el Controller hacia la View*/
    {
        public List<Estadisticas> ListaEstadisticas {get; set;} /*Guarda la lista de tarjetas de estadísticas de arriba*/
        public List<AccionesRapidas> ListaAccionesRapidas {get; set;} /*Guarda la lista de acciones rápidas clickeables*/
        public List<ActividadReciente> ListaActividadReciente {get; set;} /*Guarda la lista de actividades recientes que solo se muestran como texto plano diria yo*/
    }
}