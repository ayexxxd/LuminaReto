/*Este es el mondel de las acciones rápidas, en donde si le picas te lleva a su respectiva página*/
namespace LuminaReto.Models
{
    public class AccionesRapidas /*Crea la clase de acciones rapidas, que será el molde que va a usar accion clickeable*/
    {
        public string Texto {get; set;} /*El texto que se muestra en cada renglon*/
        public string Controlador {get; set;} /*Guarda el nombre de la sección al que te redirige*/
        public string Accion {get; set;} /*Guarda la página específica que se abrirá, o sea el index*/
        public string Icono {get; set;}

    }
}