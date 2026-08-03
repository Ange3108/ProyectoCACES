using CACES.DAL.Entidades;

namespace CACES.DAL.Repositorios.Procedimientos
{
    public interface IProcedimientosRepositorio
    {
    
        Task<List<Procedimiento>> ObtenerTodosLosProcedimientosAsync();
        Task<bool> InsertarProcedimientoAsync(Procedimiento entidad);
        Task<Procedimiento> ObtenerProcedimientoPorIdAsync(int id);
        Task<bool> ActualizarProcedimientoAdminAsync(Procedimiento entidad);
        Task<bool> CambiarEstadoProcedimientoAsync(int id);
    }
}
