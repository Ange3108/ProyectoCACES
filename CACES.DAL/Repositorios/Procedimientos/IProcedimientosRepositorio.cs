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
        Task<Cirugias?> ObtenerCirugiaParaReporteAsync(int idCirugia);
        Task<HorariosDisponibles> ObtenerHorarioPorRangoAsync(int idMedico, int diaSemana, TimeSpan hora);
        Task<List<Procedimiento>> ObtenerTodosLosProcedimientosAsync();
        Task<bool> InsertarProcedimientoAsync(Procedimiento entidad);
        Task<Procedimiento> ObtenerProcedimientoPorIdAsync(int id);
        Task<bool> ActualizarProcedimientoAdminAsync(Procedimiento entidad);
        Task<bool> CambiarEstadoProcedimientoAsync(int id);
    }
}
