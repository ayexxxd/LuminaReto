public class Formulario
{
    public string Titulo { get; set; }
    public string Descripcion { get; set; }
    public int Tokens { get; set; }
    public int Preguntas { get; set; }
    public bool DobleTokens { get; set; }
    public bool Expira { get; set; }

    public List<Preguntas> ListaPreguntas { get; set; }
}