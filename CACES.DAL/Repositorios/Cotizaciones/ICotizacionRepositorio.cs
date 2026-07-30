using CACES.DAL.Entidades;

namespace CACES.DAL.Repositorios.Cotizaciones
{
    public interface ICotizacionRepositorio
    {
        Task<Cotizacion> RegistrarAsync(Cotizacion cotizacion);

        Task<Cotizacion> ActualizarAsync(Cotizacion cotizacion);

        Task<Cotizacion?> ObtenerPorIdAsync(int idCotizacion);

        Task<List<Cotizacion>> ObtenerTodasAsync();

        Task<List<Cotizacion>> ObtenerPorPacienteAsync(int idPaciente);

        Task<List<Paciente>> ObtenerPacientesAsync();

        Task<List<Medico>> ObtenerMedicosAsync();

        Task<List<Procedimiento>> ObtenerProcedimientosAsync();

        Task<Procedimiento?> ObtenerProcedimientoPorIdAsync(int idProcedimiento);

        Task<Precios?> ObtenerPrecioMedicoAsync(int idMedico,int idProcedimiento);
        Task<ConfiguracionCotizacion?> ObtenerConfiguracionActivaAsync();


    }
}