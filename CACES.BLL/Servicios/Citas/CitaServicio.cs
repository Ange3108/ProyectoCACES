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

        public async Task<bool> RegistrarCitaAsync(Cita cita)
        {
            return await _citaRepositorio.RegistrarCitaAsync(cita);
        }

        public async Task<List<Cita>> ObtenerCitasPorPacienteAsync(int idPaciente)
        {
            return await _citaRepositorio.ObtenerCitasPorPacienteAsync(idPaciente);
        }

        public async Task<Cita?> ObtenerTicketAsync(int idCita)
        {
            return await _citaRepositorio.ObtenerTicketAsync(idCita);
        }

        public async Task<List<Cita>> ObtenerCitasPorMedicoAsync(int idMedico)
        {
            return await _citaRepositorio.ObtenerCitasPorMedicoAsync(idMedico);
        }

        public async Task<bool> CancelarCitaAsync(int idCita)
        {
            return await _citaRepositorio.CancelarCitaAsync(idCita);
        }
    }
}