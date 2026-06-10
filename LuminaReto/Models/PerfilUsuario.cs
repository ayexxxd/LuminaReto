namespace LuminaReto.Models
{
    public class PerfilUsuario
    {
        public string Correo { get; set; }
        public string Nombre { get; set; }
        public string Departamento { get; set; }
        public string url_foto { get; set; }
        public string Fecha_registro { get; set; }
        public int WhirlTokens { get; set; }
        public int RachaActual { get; set; }
        public int FormulariosTotales { get; set; }
    }
}