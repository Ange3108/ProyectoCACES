using CACES.DAL.Entidades;

namespace CACES.DAL.Repositorios.Procedimientos
{
    public interface IProcedimientosRepositorio
    {
        Task<List<Cirugias?>> ObtenerDetalleCirugiaAsync(int idPaciente);
        Task<bool> ActualizarProcedimientoAsync(Cirugias cirugias);
        Task<bool> RegistrarProcedimientosAsync(Cirugias cirugia);
        Task<List<Procedimiento>> ObtenerProcedimientosFijosAsync();
        Task<List<Cirugias>> ObtenerCirugiasPorMedicoAsync(int idMedico);
        Task<List<Cirugias>> ObtenerTodasLasCirugiasAsync();
    }
}
