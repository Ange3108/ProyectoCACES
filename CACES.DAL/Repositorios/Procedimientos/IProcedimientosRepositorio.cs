using CACES.DAL.Entidades;

namespace CACES.DAL.Repositorios.Procedimientos
{
    public interface IProcedimientosRepositorio
    {
        Task<List<Cirugias?>> ObtenerDetalleCirugiaAsync(int idPaciente);
        Task<bool> ActualizarProcedimientoAsync(Cirugias cirugias);
    }
}
