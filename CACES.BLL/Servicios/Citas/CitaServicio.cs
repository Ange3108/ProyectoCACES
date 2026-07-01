using System;
using System.Collections.Generic;
using System.Text;
using CACES.DAL.Entidades;
using CACES.DAL.Repositorios.Citas;
using CACES.DAL.Repositorios.Quirofano;

namespace CACES.BLL.Servicios.Citas
{
    public class CitaServicio : ICitaServicio
    {
        private readonly ICitaRepositorio _citaRepositorio;
        private readonly IQuirofanoRepositorio _quirofanoRepositorio;


        public CitaServicio(ICitaRepositorio citaRepositorio, IQuirofanoRepositorio quirofanoRepositorio)
        {
            _citaRepositorio = citaRepositorio;
            _quirofanoRepositorio = quirofanoRepositorio;
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
            // Validación 1: el médico opera ese día
            int diaSemana = (int)cita.Fecha.DayOfWeek;
            bool medicoDisponible = await _citaRepositorio.TieneHorarioActivoAsync(cita.IdMedico, diaSemana);
            if (!medicoDisponible)
                return false; // o retornar respuestaErrores si migras el patrón

            // Validación 2: el quirófano tiene cupo
            int citasDelDia = await _citaRepositorio.ContarCitasPorFechaAsync(cita.Fecha);
            int cupoMaximo = await _quirofanoRepositorio.GetCupoMaximoAsync();
            if (citasDelDia >= cupoMaximo) 
                return false;

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