namespace LuminaReto.Models.Formularios.Dtos;

public class PreguntaDto
{
    public int    IdPregunta    { get; set; }
    public string Texto         { get; set; } = "";
    public int    IdTipo        { get; set; }
    public string TipoRespuesta { get; set; } = "";
    public int    Orden         { get; set; }
}

public class OpcionDto
{
    public int    IdOpcion { get; set; }
    public string Texto    { get; set; } = "";
}

public class FormularioDisponibleDto
{
    public int    IdFormulario  { get; set; }
    public string Nombre        { get; set; } = "";
    public int    Tokens        { get; set; }
    public int    TotalPreguntas { get; set; }
}

public class FormularioCompletadoDto
{
    public int      IdFormulario    { get; set; }
    public string   Nombre          { get; set; } = "";
    public int      Tokens          { get; set; }
    public DateTime FechaCompletado { get; set; }
}

public class ProgresoDto
{
    public int TokensMes              { get; set; } 
    public int FormulariosCompletados { get; set; }
    public int MetaTotal              { get; set; }
}