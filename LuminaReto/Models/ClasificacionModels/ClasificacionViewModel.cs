namespace LuminaReto.Models
{
    public class ClasificacionViewModel
    {
        public List<EmpleadoRanking> Top3          { get; set; } = new();
        public List<EmpleadoRanking> Ranking       { get; set; } = new();
        public EmpleadoRanking       UsuarioActual { get; set; }
        public string                ErrorMessage  { get; set; }
    }
}