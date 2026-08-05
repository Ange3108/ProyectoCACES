using CACES.DAL.Entidades;

namespace CACES.DAL.Repositorios.Precio
{
    public interface IPrecioRepositorio
    {
        Task<List<Precios>> ObtenerTodosAsync();

        Task<Precios?> ObtenerPorIdAsync(int idPrecio);

        Task<List<Precios>> ObtenerPorMedicoIdAsync(
            int idMedico);

        Task<Precios?> ObtenerPorMedicoYProcedimientoAsync(
            int idMedico,
            int idProcedimiento);

        Task<Precios> AgregarAsync(
            Precios precio);

        Task<Precios> ActualizarAsync(
            Precios precio);
    }
}