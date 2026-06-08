/*Este es el mondel de las actividades recientes, esto se irá actualizando con la base de datos cuando esta se actualice*/
namespace LuminaReto.Models
{
    public class ActividadReciente /*Crea la clase de actividad reciente, que será el molde de lo que se irá actualizando pero ya con la BD, pero ahorita no se actualizará y solo será texto plano*/
    {
        public string Descripcion {get; set;} /*guarda la acción que acaba de pasar*/
        public string Tiempo {get; set;} /*el tiempo que lleva de que pasó eso*/
        public string Icono {get; set;}

    }
}
