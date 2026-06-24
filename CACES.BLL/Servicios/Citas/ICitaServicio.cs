using System;
using System.Collections.Generic;
using System.Text;
using CACES.DAL.Entidades;

namespace CACES.BLL.Servicios.Citas
{
    public interface ICitaServicio
    {
        Task<List<Cita>> GetCitasAsync();

        Task<bool> ActualizarFechaCitaAsync(int idCita, DateTime nuevaFecha);

        Task<bool> CancelarCitasPorMedicoYFechaAsync(int idMedico, DateTime fechaCita);

        Task<bool> RegistrarCitaAsync(CACES.DAL.Entidades.Cita cita);

        Task<List<Cita>> ObtenerCitasPorPacienteAsync(int idPaciente);

        Task<Cita?> ObtenerTicketAsync(int idCita);

        Task<List<Cita>> ObtenerCitasPorMedicoAsync(int idMedico);

        Task<bool> CancelarCitaAsync(int idCita);
    }
}