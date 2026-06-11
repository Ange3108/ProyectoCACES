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
    }
}