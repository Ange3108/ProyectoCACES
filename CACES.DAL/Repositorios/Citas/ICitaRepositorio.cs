using CACES.DAL.Entidades;

namespace CACES.DAL.Repositorios.Citas
{
    public interface ICitaRepositorio
    {
        Task<List<Cita>> ObtenerTodasAsync();
        Task<List<Cita>> ObtenerPorPacienteAsync(int idPaciente);
        Task<List<Cita>> ObtenerPorMedicoAsync(int idMedico);
        Task<Cita?> ObtenerEntidadPorIdAsync(int idCita);

        Task<Cita> RegistrarAsync(Cita cita);
        Task<Cita> ActualizarAsync(Cita cita);

        Task<bool> ExisteCitaAsync(int idMedico, DateTime fecha, TimeSpan hora, int? excluir = null);

        Task<List<Medico>> ObtenerMedicosAsync();
        Task<List<Especialidad>> ObtenerEspecialidadesAsync();
        Task<List<HorariosDisponibles>> ObtenerHorariosAsync(int idMedico);

        Task<int> ContarCitasPorFechaAsync(DateTime fecha);
        Task<bool> TieneHorarioActivoAsync(int idMedico, int diaSemana);
    }
}