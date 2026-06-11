using System;
using System.Collections.Generic;
using System.Text;
using CACES.DAL.Entidades;
using CACES.DAL.Repositorios.Citas;

namespace CACES.BLL.Servicios.Citas
{
    public class CitaServicio : ICitaServicio
    {
        private readonly ICitaRepositorio _citaRepositorio;

        public CitaServicio(ICitaRepositorio citaRepositorio)
        {
            _citaRepositorio = citaRepositorio;
        }

        public async Task<List<Cita>> GetCitasAsync()
        {
            return await _citaRepositorio.GetCitasAsync();
        }

        public async Task<bool> ActualizarFechaCitaAsync(int idCita, DateTime nuevaFecha)
        {
            return await _citaRepositorio.ActualizarFechaCitaAsync(idCita, nuevaFecha);
        }

        public async Task<bool> CancelarCitasPorMedicoYFechaAsync(int idMedico, DateTime fechaCita)
        {
            return await _citaRepositorio.CancelarCitasPorMedicoYFechaAsync(idMedico, fechaCita);
        }
    }
}