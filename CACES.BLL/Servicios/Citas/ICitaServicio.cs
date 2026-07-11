using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Cita;

namespace CACES.BLL.Servicios.Citas
{
    public interface ICitaServicio
    {
        Task<respuestaErrores<List<MostrarCitaDTO>>> GetCitasAsync();

        Task<respuestaErrores<MostrarCitaDTO>> RegistrarCitaAsync(RegistrarCitaDTO dto, int idPaciente);

        Task<respuestaErrores<List<MostrarCitaDTO>>> ObtenerCitasPorPacienteAsync(int idPaciente);

        Task<respuestaErrores<List<MostrarCitaDTO>>> ObtenerCitasPorMedicoAsync(int idMedico);

        Task<respuestaErrores<List<MostrarCitaDTO>>> ObtenerCitasPorUsuarioMedicoAsync(int idUsuario);

        Task<respuestaErrores<MostrarCitaDTO>> ObtenerTicketAsync(int idCita);

        Task<respuestaErrores<MostrarCitaDTO>> CancelarCitaAsync(int idCita);

        Task<respuestaErrores<MostrarCitaDTO>> ActualizarFechaCitaAsync(int idCita, DateTime nuevaFecha);

        Task<respuestaErrores<MostrarCitaDTO>> CancelarCitasPorMedicoYFechaAsync(int idMedico, DateTime fechaCita);

        Task<respuestaErrores<List<CitaComboDTO>>> ObtenerMedicosAsync();

        Task<respuestaErrores<List<CitaComboDTO>>> ObtenerEspecialidadesActivasAsync();

        Task<respuestaErrores<List<CitaHorarioDTO>>> ObtenerHorariosPorMedicoAsync(int idMedico);
    }
}