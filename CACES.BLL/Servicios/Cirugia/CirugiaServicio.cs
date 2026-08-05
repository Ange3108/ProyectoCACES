using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Cirugia;
using CACES.BLL.DTOs.Cita;
using CACES.BLL.DTOs.Especialidad;
using CACES.BLL.Mappers;
using CACES.BLL.Servicios.Citas;
using CACES.BLL.Servicios.SeguimientoPaciente;
using CACES.DAL.Entidades;
using CACES.DAL.Repositorios.Base;
using CACES.DAL.Repositorios.Cirugia;

using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.Servicios.Cirugia
{
    public class CirugiaServicio : ICirugiaServicio

    {
        private readonly IRepositorioGenerico<Cirugias> _repositorioGenerico;
        private readonly ICirugiaRepositorio _cirugiaRepositorio;
        private readonly ISeguimientoPacienteServicio _seguimientoPacienteServicio;

        public CirugiaServicio(IRepositorioGenerico<Cirugias> repositorioGenerico, ICirugiaRepositorio cirugiaRepositorio,  ISeguimientoPacienteServicio seguimientoPacienteServicio)
        {
            _repositorioGenerico = repositorioGenerico;
            _cirugiaRepositorio = cirugiaRepositorio;

            _seguimientoPacienteServicio = seguimientoPacienteServicio;

        }
        public async Task<respuestaErrores<MostrarCirugiaDTO>> ActualizarCirugiaAsync(int id, CirugiaDTO cirugia)
        {
            var respuesta = new respuestaErrores<MostrarCirugiaDTO>();
            var cirugiaExistente = await _repositorioGenerico.ObtenerPorIdAsync(id);
            if (cirugiaExistente == null)
            {
                respuesta.EsCorrecto = false;
                respuesta.codigo = 400;
                respuesta.mensaje = "Cirugia no encontrada";
                return respuesta;
            }

            cirugiaExistente.Estado = cirugia.Estado;

            await _repositorioGenerico.Actualizar(cirugiaExistente);



            respuesta.EsCorrecto = true;
            respuesta.mensaje = "Cirugia actualizada exitosamente";
            respuesta.Dato = cirugiaExistente.ToMostrarCirugiaDTO();
            return respuesta;
           
        }

        public async Task<respuestaErrores<MostrarCitaDTO>> CancelarCirugiaAsync(int idCirugia)
        {
            var respuesta = new respuestaErrores<MostrarCitaDTO>();
            var cirugiaExistente = await _repositorioGenerico.ObtenerPorIdAsync(idCirugia);
            if(cirugiaExistente == null)
            {
                respuesta.EsCorrecto = false;
                respuesta.codigo = 400;
                respuesta.mensaje = "Cirugia no encontrada";
                return respuesta;
            }
            cirugiaExistente.Estado = EstadoCirugia.Canelada;
            await _repositorioGenerico.Actualizar(cirugiaExistente);

            respuesta.EsCorrecto = true;
            respuesta.codigo = 200;
            respuesta.mensaje = "Cirugia actualizada exitosamente";
            return respuesta;
        }

        public async Task<respuestaErrores<MostrarCirugiaDTO>> CompletarCirugia(int id)
        {
            var respuesta = new respuestaErrores<MostrarCirugiaDTO>();
            var resultado = await _cirugiaRepositorio.CambiarEstadoAsync(id);

            if (!resultado)
            {
                respuesta.EsCorrecto = false;
                respuesta.codigo = 400;
                respuesta.mensaje = "No se pudo completar la cirugia";
                return respuesta;
            }
            await _seguimientoPacienteServicio.GenerarCheckpoints(id);

            respuesta.EsCorrecto = true;
            respuesta.codigo = 201;
            respuesta.mensaje = "Cirugia completada";
            return respuesta;
        }

        

        public async Task<respuestaErrores<List<MostrarCirugiaDTO>>> GetAllCirugiaAsync()
        {
            var respuesta = new respuestaErrores<List<MostrarCirugiaDTO>>();

            var cirugias = await _repositorioGenerico.ObtenerTodosAsync();

            respuesta.EsCorrecto = true;
            respuesta.codigo = 200;
            respuesta.mensaje = "Cirugias obtenidas exitosamente";
            respuesta.Dato = cirugias.Select(c => c.ToMostrarCirugiaDTO()).ToList();
            return respuesta;
        }

        public async Task<respuestaErrores<MostrarCirugiaDTO>> GetCirugiaByIdAsync(int id)
        {
            var respuesta = new respuestaErrores<MostrarCirugiaDTO>();

            var cirugiaExistente = await _repositorioGenerico.ObtenerPorIdAsync(id);
            if (cirugiaExistente == null)
            {
                respuesta.EsCorrecto = false;
                respuesta.codigo = 400;
                respuesta.mensaje = "Cirugia no encontrada";
                return respuesta;
            }

            respuesta.EsCorrecto = true;
            respuesta.codigo = 200;
            respuesta.mensaje = "Cirugia obtenida exitosamente";
            respuesta.Dato = cirugiaExistente.ToMostrarCirugiaDTO();
            return respuesta;
        }

        public async Task<respuestaErrores<List<MostrarCirugiaDTO>>> ObtenerCirugiaPorPacienteAsync(int idPaciente)
        {
            var lista = await _cirugiaRepositorio.conseguirCirugiaPorPAciente(idPaciente);
            return new respuestaErrores<List<MostrarCirugiaDTO>>
            {
                EsCorrecto = true,
                codigo = 200,
                mensaje = "Cirugias obtenidas exitosamente",
                Dato = lista.Select(c => c.ToMostrarCirugiaDTO()).ToList()
            };
        }
    }
}
