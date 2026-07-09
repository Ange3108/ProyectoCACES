using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Cita;
using CACES.DAL.Entidades;

namespace CACES.BLL.Servicios.Citas
{
    public interface ICitaServicio
    {
        Task<respuestaErrores<List<MostrarCitaDTO>>> GetCitasAsync();
        Task<respuestaErrores<MostrarCitaDTO>> ActualizarFechaCitaAsync(int idCita, DateTime nuevaFecha);
        Task<respuestaErrores<MostrarCitaDTO>> CancelarCitasPorMedicoYFechaAsync(int idMedico, DateTime fechaCita);


        Task<respuestaErrores<MostrarCitaDTO>> RegistrarCitaAsync(RegistrarCitaDTO dto, int idPaciente);

        Task<respuestaErrores<List<MostrarCitaDTO>>> ObtenerCitasPorPacienteAsync(int idPaciente);
        Task<respuestaErrores<MostrarCitaDTO>> ObtenerTicketAsync(int idCita);
        Task<respuestaErrores<List<MostrarCitaDTO>>> ObtenerCitasPorMedicoAsync(int idMedico);
        Task<respuestaErrores<MostrarCitaDTO>> CancelarCitaAsync(int idCita);
    }
}