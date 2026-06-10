namespace LuminaReto.Models.Formularios.ViewModels;

public class FormularioVm
{
    public int    IdFormulario     { get; set; }
    public string Titulo           { get; set; } = "";
    public string Descripcion      { get; set; } = "";
    public int    Tokens           { get; set; }
    public int    Preguntas        { get; set; }
    public bool   DobleTokens      { get; set; }
    public string ImagenFormulario { get; set; } = "";
}