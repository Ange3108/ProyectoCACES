using System;
using System.Collections.Generic;
using System.Text;
using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Cita;
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

        public async Task<respuestaErrores<MostrarCitaDTO>> ActualizarFechaCitaAsync(int idCita, DateTime nuevaFecha)
        {
            var respuesta = new respuestaErrores<MostrarCitaDTO>();
            if (respuesta == null)
            {
                respuesta.EsCorrecto = false;
                respuesta.mensaje = "No se pudo realizar la acción.";
                respuesta.codigo = 404;
                return respuesta;
            }
            var citaExisente = await _citaRepositorio.GetCitaByIdAsync(idCita);
            if (citaExisente == null)
            {
                respuesta.EsCorrecto = false;
                respuesta.mensaje = "No se pudo obtener la cita.";
                respuesta.codigo = 404;
                return respuesta;
            }
            citaExisente.Fecha = nuevaFecha;
            citaExisente.FechaDeModificacion = DateTime.Now;
            citaExisente.Estado = 1; // Estado actualizado
            bool exito = await _citaRepositorio.ActualizarFechaCitaAsync(idCita, nuevaFecha);
            if (!exito)
            {
                respuesta.EsCorrecto = false;
                respuesta.mensaje = "No se pudo actualizar la fecha de la cita.";
                respuesta.codigo = 500;
                return respuesta;
            }    
            respuesta.EsCorrecto = true;
            respuesta.mensaje = "Fecha de la cita actualizada correctamente.";
            respuesta.codigo = 200;
            return respuesta;
        }

        public Task<respuestaErrores<MostrarCitaDTO>> CancelarCitaAsync(int idCita)
        {
            throw new NotImplementedException();
        }

        public Task<respuestaErrores<MostrarCitaDTO>> CancelarCitasPorMedicoYFechaAsync(int idMedico, DateTime fechaCita)
        {
            throw new NotImplementedException();
        }

        public Task<respuestaErrores<List<MostrarCitaDTO>>> GetCitasAsync()
        {
            throw new NotImplementedException();
        }

        public Task<respuestaErrores<List<MostrarCitaDTO>>> ObtenerCitasPorMedicoAsync(int idMedico)
        {
            throw new NotImplementedException();
        }

        public Task<respuestaErrores<List<MostrarCitaDTO>>> ObtenerCitasPorPacienteAsync(int idPaciente)
        {
            throw new NotImplementedException();
        }

        public Task<respuestaErrores<MostrarCitaDTO>> ObtenerTicketAsync(int idCita)
        {
            throw new NotImplementedException();
        }

        public Task<respuestaErrores<MostrarCitaDTO>> RegistrarCitaAsync(RegistrarCitaDTO dto, int idPaciente)
        {
            throw new NotImplementedException();
        }
    }
}