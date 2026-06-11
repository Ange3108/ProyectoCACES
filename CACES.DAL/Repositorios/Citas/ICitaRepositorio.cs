using System;
using System.Collections.Generic;
using System.Text;

using CACES.DAL.Entidades;

namespace CACES.DAL.Repositorios.Citas
{
    public interface ICitaRepositorio
    {
        Task<List<Cita>> GetCitasAsync();

        Task<Cita?> GetCitaByIdAsync(int idCita);

        Task<bool> ActualizarFechaCitaAsync(int idCita, DateTime nuevaFecha);

        Task<bool> CancelarCitasPorMedicoYFechaAsync(int idMedico, DateTime fechaCita);
    }
}