using CACES.BLL.DTOs;
using CACES.BLL.DTOs.SeguimientoPostOperatorio;
using CACES.BLL.Mappers;
using CACES.DAL.Entidades.SeguimientoPostOperatorio;
using CACES.DAL.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.Servicios.RespuestaSeguimiento
{
    public class RespuestaSeguimientoServicio : IRespuestaSeguimientoServicio
    {
        private readonly IRepositorioGenerico<DAL.Entidades.SeguimientoPostOperatorio.RespuestaSeguimiento> _repositorioRespuestas;
        private readonly IRepositorioGenerico<PreguntaSeguimiento> _repositorioPreguntas;
        private readonly IRepositorioGenerico<DAL.Entidades.SeguimientoPostOperatorio.SeguimientoPaciente> _repositorioSeguimiento;
        private readonly IRepositorioGenerico<DAL.Entidades.SeguimientoPostOperatorio.AlertaStaff> _repositorioAlertas;

        public RespuestaSeguimientoServicio(
            IRepositorioGenerico<DAL.Entidades.SeguimientoPostOperatorio.RespuestaSeguimiento> repositorioRespuestas,
            IRepositorioGenerico<PreguntaSeguimiento> repositorioPreguntas,
            IRepositorioGenerico<DAL.Entidades.SeguimientoPostOperatorio.SeguimientoPaciente> repositorioSeguimiento,
            IRepositorioGenerico<DAL.Entidades.SeguimientoPostOperatorio.AlertaStaff> repositorioAlertas)
        {
            _repositorioRespuestas = repositorioRespuestas;
            _repositorioPreguntas = repositorioPreguntas;
            _repositorioSeguimiento = repositorioSeguimiento;
            _repositorioAlertas = repositorioAlertas;
        }
        public async Task<respuestaErrores<List<MostrarRespuestaSeguimientoDTO>>> ObtenerTodas()
        {
            var respuesta = new respuestaErrores<List<MostrarRespuestaSeguimientoDTO>>();
            var lista = await _repositorioRespuestas.ObtenerTodosAsync();
            var dtos = lista.Select(r => r.ToMostrarRespuestaSeguimientoDTO()!).ToList();

            respuesta.EsCorrecto = true;
            respuesta.Dato = dtos;
            respuesta.mensaje = "Respuestas obtenidas correctamente";
            respuesta.codigo = 200;
            return respuesta;
        }
        public async Task<respuestaErrores<List<MostrarRespuestaSeguimientoDTO>>> ObtenerPorSeguimiento(int idSeguimiento)
        {
            var respuesta = new respuestaErrores<List<MostrarRespuestaSeguimientoDTO>>();
            var lista = await _repositorioRespuestas.BuscarTodos(r => r.IdSeguimiento == idSeguimiento);
            var dtos = lista.Select(r => r.ToMostrarRespuestaSeguimientoDTO()!).ToList();

            respuesta.EsCorrecto = true;
            respuesta.Dato = dtos;
            respuesta.mensaje = "Respuestas obtenidas correctamente";
            respuesta.codigo = 200;
            return respuesta;
        }

        public async Task<respuestaErrores<List<MostrarRespuestaSeguimientoDTO>>> RegistrarRespuestas(List<RegistrarRespuestaSeguimientoDTO> respuestasDTO)
        {
            var respuesta = new respuestaErrores<List<MostrarRespuestaSeguimientoDTO>>();

            if (respuestasDTO == null || !respuestasDTO.Any())
            {
                respuesta.EsCorrecto = false;
                respuesta.mensaje = "No se recibieron respuestas para registrar";
                respuesta.codigo = 400;
                return respuesta;
            }

            var idSeguimiento = respuestasDTO.First().IdSeguimiento;
            var seguimiento = await _repositorioSeguimiento.ObtenerPorIdAsync(idSeguimiento);
            if (seguimiento == null)
            {
                respuesta.EsCorrecto = false;
                respuesta.mensaje = "No se encontró el seguimiento asociado";
                respuesta.codigo = 404;
                return respuesta;
            }

            bool huboAlerta = false;
            var entidadesCreadas = new List<DAL.Entidades.SeguimientoPostOperatorio.RespuestaSeguimiento>();

            foreach (var dto in respuestasDTO)
            {
                var pregunta = await _repositorioPreguntas.ObtenerPorIdAsync(dto.IdPregunta);
                if (pregunta == null) continue;

                bool generoAlerta = pregunta.DireccionAlerta == DireccionAlerta.MayorIgual
                    ? dto.ValorRespuesta >= pregunta.UmbralAlerta
                    : dto.ValorRespuesta <= pregunta.UmbralAlerta;

                if (generoAlerta) huboAlerta = true;

                var nuevaRespuesta = new DAL.Entidades.SeguimientoPostOperatorio.RespuestaSeguimiento
                {
                    IdSeguimiento = dto.IdSeguimiento,
                    IdPregunta = dto.IdPregunta,
                    ValorRespuesta = dto.ValorRespuesta,
                    GeneroAlerta = generoAlerta
                };

                await _repositorioRespuestas.Crear(nuevaRespuesta);
                entidadesCreadas.Add(nuevaRespuesta);
            }

            seguimiento.Estado = huboAlerta ? EstadoSeguimiento.RequiereAtencion : EstadoSeguimiento.Completado;
            seguimiento.FechaRegistro = DateTime.UtcNow;
            await _repositorioSeguimiento.Actualizar(seguimiento);

            if (huboAlerta)
            {
                var alerta = new DAL.Entidades.SeguimientoPostOperatorio.AlertaStaff
                {
                    IdSeguimiento = idSeguimiento,
                    FechaGenerada = DateTime.UtcNow,
                    Estado = EstadoAlerta.Pendiente
                };

                await _repositorioAlertas.Crear(alerta);
            }

            await _repositorioRespuestas.GuardarCambiosAsync();

            var dtosCreados = entidadesCreadas.Select(e => e.ToMostrarRespuestaSeguimientoDTO()!).ToList();

            respuesta.EsCorrecto = true;
            respuesta.Dato = dtosCreados;
            respuesta.mensaje = huboAlerta
                ? "Respuestas registradas. Se generó una alerta para el staff."
                : "Respuestas registradas correctamente.";
            respuesta.codigo = 200;
            return respuesta;
        }

       
    }
}
