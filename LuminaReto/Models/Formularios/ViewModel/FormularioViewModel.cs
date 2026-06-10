using LuminaReto.Models.Formularios.ViewModels;

namespace LuminaReto.Models.Formularios.ViewModels;

public class FormulariosViewModel
{
    public List<FormularioVm> Formularios                { get; set; } = new();
    public List<FormularioVm> FormulariosCompletadosLista { get; set; } = new();
    public int TokensMes              { get; set; }
    public int FormulariosCompletados { get; set; }
    public int MetaTotal              { get; set; }
}