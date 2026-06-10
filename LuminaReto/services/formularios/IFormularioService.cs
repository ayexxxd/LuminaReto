using LuminaReto.Models.Formularios.Dtos;

namespace LuminaReto.Services.Formularios;

public interface IFormularioService
{
    Task<List<PreguntaDto>>              ObtenerPreguntasAsync(int idFormulario);
    Task<List<OpcionDto>>                ObtenerOpcionesAsync(int idPregunta);
    Task<List<FormularioDisponibleDto>>  ObtenerFormulariosDisponiblesAsync(int idUsuario);
    Task<List<FormularioCompletadoDto>>  ObtenerFormulariosCompletadosAsync(int idUsuario);
    Task<ProgresoDto>                    ObtenerProgresoAsync(int idUsuario);
    Task                                 CompletarFormularioAsync(int idUsuario, int idFormulario);
}